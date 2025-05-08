using Xunit;
using Moq;
using StudentManagement.Services;
using StudentManagement.Repositories;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Services
{
    public class StudentStatusServiceTests
    {
        private readonly Mock<IStudentStatusRepository> _mockRepository;
        private readonly IStudentStatusService _service;

        public StudentStatusServiceTests()
        {
            _mockRepository = new Mock<IStudentStatusRepository>();
            _service = new StudentStatusService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllStudentStatusesAsync_ShouldReturnAllStatuses()
        {
            // Arrange
            var expectedStatuses = new List<StudentStatus>
            {
                new StudentStatus { Id = 1, Name = "Đang học" },
                new StudentStatus { Id = 2, Name = "Đã tốt nghiệp" },
                new StudentStatus { Id = 3, Name = "Đã thôi học" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedStatuses);

            // Act
            var result = await _service.GetAllStudentStatusesAsync();

            // Assert
            Assert.Equal(expectedStatuses, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetStudentStatusByIdAsync_ExistingStatus_ShouldReturnStatus()
        {
            // Arrange
            var statusId = 1;
            var expectedStatus = new StudentStatus { Id = statusId, Name = "Đang học" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(statusId))
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await _service.GetStudentStatusByIdAsync(statusId);

            // Assert
            Assert.Equal(expectedStatus, result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(statusId), Times.Once);
        }

        [Fact]
        public async Task GetStudentStatusByIdAsync_NonExistentStatus_ShouldReturnNull()
        {
            // Arrange
            var statusId = 999;
            _mockRepository.Setup(repo => repo.GetByIdAsync(statusId))
                .ReturnsAsync((StudentStatus)null);

            // Act
            var result = await _service.GetStudentStatusByIdAsync(statusId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(statusId), Times.Once);
        }

        [Fact]
        public async Task CreateStudentStatusAsync_WithNewName_ShouldCreateStatus()
        {
            // Arrange
            var newStatus = new StudentStatus { Name = "Tạm dừng học" };
            _mockRepository.Setup(repo => repo.ExistsAsync(newStatus.Name))
                .ReturnsAsync(false);
            _mockRepository.Setup(repo => repo.AddAsync(newStatus))
                .ReturnsAsync(newStatus);

            // Act
            var result = await _service.CreateStudentStatusAsync(newStatus);

            // Assert
            Assert.Equal(newStatus, result);
            _mockRepository.Verify(repo => repo.ExistsAsync(newStatus.Name), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(newStatus), Times.Once);
        }

        [Fact]
        public async Task CreateStudentStatusAsync_WithExistingName_ShouldReturnNull()
        {
            // Arrange
            var newStatus = new StudentStatus { Name = "Đang học" };
            _mockRepository.Setup(repo => repo.ExistsAsync(newStatus.Name))
                .ReturnsAsync(true);

            // Act
            var result = await _service.CreateStudentStatusAsync(newStatus);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.ExistsAsync(newStatus.Name), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<StudentStatus>()), Times.Never);
        }

        [Fact]
        public async Task UpdateStudentStatusAsync_WithValidData_ShouldSucceed()
        {
            // Arrange
            var statusId = 1;
            var updatedStatus = new StudentStatus { Id = statusId, Name = "Đang học (Cập nhật)" };
            _mockRepository.Setup(repo => repo.UpdateAsync(updatedStatus))
                .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateStudentStatusAsync(statusId, updatedStatus);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(updatedStatus), Times.Once);
        }

        [Fact]
        public async Task UpdateStudentStatusAsync_WithMismatchedId_ShouldFail()
        {
            // Arrange
            var statusId = 1;
            var updatedStatus = new StudentStatus { Id = 2, Name = "Đang học (Cập nhật)" };

            // Act
            var result = await _service.UpdateStudentStatusAsync(statusId, updatedStatus);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<StudentStatus>()), Times.Never);
        }

        [Fact]
        public async Task DeleteStudentStatusAsync_WithNoAssociatedStudents_ShouldSucceed()
        {
            // Arrange
            var statusId = 1;
            _mockRepository.Setup(repo => repo.DeleteAsync(statusId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteStudentStatusAsync(statusId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(statusId), Times.Once);
        }

        [Fact]
        public async Task DeleteStudentStatusAsync_WithAssociatedStudents_ShouldFail()
        {
            // Arrange
            var statusId = 1;
            _mockRepository.Setup(repo => repo.DeleteAsync(statusId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteStudentStatusAsync(statusId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(statusId), Times.Once);
        }
    }
} 