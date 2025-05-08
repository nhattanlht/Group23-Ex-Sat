using Xunit;
using Moq;
using StudentManagement.Services;
using StudentManagement.Repositories;
using StudentManagement.Models;
using StudentManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Services
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly IStudentService _studentService;

        public StudentServiceTests()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _studentService = new StudentService(_mockStudentRepository.Object);
        }

        [Fact]
        public async Task GetStudents_ShouldReturnCorrectPagination()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var totalStudents = 25;
            var expectedStudents = new List<Student>
            {
                new Student { MSSV = "SV001", HoTen = "Nguyen Van A" },
                new Student { MSSV = "SV002", HoTen = "Tran Van B" }
            };

            _mockStudentRepository.Setup(repo => repo.GetStudentsCount())
                .ReturnsAsync(totalStudents);
            _mockStudentRepository.Setup(repo => repo.GetStudents(page, pageSize))
                .ReturnsAsync(expectedStudents);

            // Act
            var result = await _studentService.GetStudents(page, pageSize);
            var students = result.Item1;
            var total = result.Item2;
            var totalPages = result.Item3;

            // Assert
            Assert.Equal(expectedStudents, students);
            Assert.Equal(totalStudents, total);
            Assert.Equal(3, totalPages); // Ceiling(25/10) = 3
        }

        [Fact]
        public async Task GetStudentById_ExistingStudent_ShouldReturnStudent()
        {
            // Arrange
            var studentId = "SV001";
            var expectedStudent = new Student
            {
                MSSV = studentId,
                HoTen = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            _mockStudentRepository.Setup(repo => repo.GetStudentById(studentId))
                .ReturnsAsync(expectedStudent);

            // Act
            var result = await _studentService.GetStudentById(studentId);

            // Assert
            Assert.Equal(expectedStudent, result);
        }

        [Fact]
        public async Task CreateStudent_WithValidData_ShouldSucceed()
        {
            // Arrange
            var student = new Student
            {
                MSSV = "SV001",
                HoTen = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            _mockStudentRepository.Setup(repo => repo.StudentExistsByPhoneNumber(student.SoDienThoai, null))
                .ReturnsAsync(false);
            _mockStudentRepository.Setup(repo => repo.StudentExistsByEmail(student.Email, null))
                .ReturnsAsync(false);
            _mockStudentRepository.Setup(repo => repo.CreateStudent(student))
                .ReturnsAsync(true);

            // Act
            var (success, message) = await _studentService.CreateStudent(student);

            // Assert
            Assert.True(success);
            Assert.Equal("Sinh viên được tạo thành công.", message);
        }

        [Fact]
        public async Task CreateStudent_WithInvalidPhone_ShouldFail()
        {
            // Arrange
            var student = new Student
            {
                MSSV = "SV001",
                HoTen = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                SoDienThoai = "123" // Invalid phone number
            };

            // Act
            var (success, message) = await _studentService.CreateStudent(student);

            // Assert
            Assert.False(success);
            Assert.Equal("Số điện thoại không hợp lệ.", message);
        }

        [Fact]
        public async Task CreateStudent_WithDuplicatePhone_ShouldFail()
        {
            // Arrange
            var student = new Student
            {
                MSSV = "SV001",
                HoTen = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            _mockStudentRepository.Setup(repo => repo.StudentExistsByPhoneNumber(student.SoDienThoai, null))
                .ReturnsAsync(true);

            // Act
            var (success, message) = await _studentService.CreateStudent(student);

            // Assert
            Assert.False(success);
            Assert.Equal("Số điện thoại đã tồn tại trong hệ thống.", message);
        }

        [Fact]
        public async Task CreateStudent_WithInvalidEmail_ShouldFail()
        {
            // Arrange
            var student = new Student
            {
                MSSV = "SV001",
                HoTen = "Nguyen Van A",
                Email = "invalid-email",
                SoDienThoai = "0987654321"
            };

            // Act
            var (success, message) = await _studentService.CreateStudent(student);

            // Assert
            Assert.False(success);
            Assert.Equal("Email không hợp lệ.", message);
        }

        [Fact]
        public async Task CreateStudent_WithDuplicateEmail_ShouldFail()
        {
            // Arrange
            var student = new Student
            {
                MSSV = "SV001",
                HoTen = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            _mockStudentRepository.Setup(repo => repo.StudentExistsByPhoneNumber(student.SoDienThoai, null))
                .ReturnsAsync(false);
            _mockStudentRepository.Setup(repo => repo.StudentExistsByEmail(student.Email, null))
                .ReturnsAsync(true);

            // Act
            var (success, message) = await _studentService.CreateStudent(student);

            // Assert
            Assert.False(success);
            Assert.Equal("Email đã tồn tại trong hệ thống.", message);
        }

        [Fact]
        public async Task UpdateStudent_WithValidStatusTransition_ShouldSucceed()
        {
            // Arrange
            var existingStudent = new Student
            {
                MSSV = "SV001",
                StatusId = 1, // Đang học
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            var updatedStudent = new Student
            {
                MSSV = "SV001",
                StatusId = 4, // Tạm dừng học
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            _mockStudentRepository.Setup(repo => repo.GetStudentById(updatedStudent.MSSV))
                .ReturnsAsync(existingStudent);
            _mockStudentRepository.Setup(repo => repo.StudentExistsByPhoneNumber(updatedStudent.SoDienThoai, updatedStudent.MSSV))
                .ReturnsAsync(false);
            _mockStudentRepository.Setup(repo => repo.StudentExistsByEmail(updatedStudent.Email, updatedStudent.MSSV))
                .ReturnsAsync(false);
            _mockStudentRepository.Setup(repo => repo.UpdateStudent(updatedStudent))
                .ReturnsAsync(true);

            // Act
            var (success, message) = await _studentService.UpdateStudent(updatedStudent);

            // Assert
            Assert.True(success);
            Assert.Equal("Cập nhật thông tin sinh viên thành công.", message);
        }

        [Fact]
        public async Task UpdateStudent_WithInvalidStatusTransition_ShouldFail()
        {
            // Arrange
            var existingStudent = new Student
            {
                MSSV = "SV001",
                StatusId = 2, // Đã tốt nghiệp
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            var updatedStudent = new Student
            {
                MSSV = "SV001",
                StatusId = 1, // Đang học
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            _mockStudentRepository.Setup(repo => repo.GetStudentById(updatedStudent.MSSV))
                .ReturnsAsync(existingStudent);

            // Act
            var (success, message) = await _studentService.UpdateStudent(updatedStudent);

            // Assert
            Assert.False(success);
            Assert.Contains("Không thể chuyển đổi trạng thái sinh viên", message);
        }

        [Fact]
        public async Task UpdateStudent_NonExistentStudent_ShouldFail()
        {
            // Arrange
            var student = new Student
            {
                MSSV = "SV001",
                HoTen = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                SoDienThoai = "0987654321"
            };

            _mockStudentRepository.Setup(repo => repo.GetStudentById(student.MSSV))
                .ReturnsAsync((Student)null);

            // Act
            var (success, message) = await _studentService.UpdateStudent(student);

            // Assert
            Assert.False(success);
            Assert.Equal("Sinh viên không tồn tại.", message);
        }

        [Fact]
        public async Task DeleteStudent_ExistingStudent_ShouldSucceed()
        {
            // Arrange
            var studentId = "SV001";
            _mockStudentRepository.Setup(repo => repo.DeleteStudent(studentId))
                .ReturnsAsync(true);

            // Act
            var result = await _studentService.DeleteStudent(studentId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SearchStudents_ShouldReturnFilteredResults()
        {
            // Arrange
            var filters = new StudentFilterModel { Keyword = "Nguyen" };
            var page = 1;
            var pageSize = 10;
            var expectedStudents = new List<Student>
            {
                new Student { MSSV = "SV001", HoTen = "Nguyen Van A" },
                new Student { MSSV = "SV002", HoTen = "Nguyen Van B" }
            };

            _mockStudentRepository.Setup(repo => repo.SearchStudents(filters.Keyword, page, pageSize))
                .ReturnsAsync(expectedStudents);
            _mockStudentRepository.Setup(repo => repo.GetStudentsCount())
                .ReturnsAsync(2);

            // Act
            var result = await _studentService.SearchStudents(filters, page, pageSize);
            var students = result.Item1;
            var total = result.Item2;
            var totalPages = result.Item3;

            // Assert
            Assert.Equal(expectedStudents, students);
            Assert.Equal(2, total);
            Assert.Equal(1, totalPages);
        }
    }
} 