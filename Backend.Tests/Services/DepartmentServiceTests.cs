using Xunit;
using Moq;
using StudentManagement.Services;
using StudentManagement.Repositories;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Services
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
        private readonly IDepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _mockDepartmentRepository = new Mock<IDepartmentRepository>();
            _departmentService = new DepartmentService(_mockDepartmentRepository.Object);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            var expectedDepartments = new List<Department>
            {
                new Department { Id = 1, Name = "Khoa Công nghệ thông tin" },
                new Department { Id = 2, Name = "Khoa Điện - Điện tử" }
            };

            _mockDepartmentRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedDepartments);

            // Act
            var result = await _departmentService.GetAllDepartmentsAsync();

            // Assert
            Assert.Equal(expectedDepartments, result);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ExistingDepartment_ShouldReturnDepartment()
        {
            // Arrange
            var departmentId = 1;
            var expectedDepartment = new Department { Id = departmentId, Name = "Khoa Công nghệ thông tin" };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync(expectedDepartment);

            // Act
            var result = await _departmentService.GetDepartmentByIdAsync(departmentId);

            // Assert
            Assert.Equal(expectedDepartment, result);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_NonExistentDepartment_ShouldReturnNull()
        {
            // Arrange
            var departmentId = 999;
            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync((Department)null);

            // Act
            var result = await _departmentService.GetDepartmentByIdAsync(departmentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckDuplicateAsync_WithNewName_ShouldReturnFalse()
        {
            // Arrange
            var departmentName = "Khoa Công nghệ thông tin";
            _mockDepartmentRepository.Setup(repo => repo.ExistsByNameAsync(departmentName))
                .ReturnsAsync(false);

            // Act
            var (exists, message) = await _departmentService.CheckDuplicateAsync(departmentName);

            // Assert
            Assert.False(exists);
            Assert.Empty(message);
        }

        [Fact]
        public async Task CheckDuplicateAsync_WithExistingName_ShouldReturnTrue()
        {
            // Arrange
            var departmentName = "Khoa Công nghệ thông tin";
            _mockDepartmentRepository.Setup(repo => repo.ExistsByNameAsync(departmentName))
                .ReturnsAsync(true);

            // Act
            var (exists, message) = await _departmentService.CheckDuplicateAsync(departmentName);

            // Assert
            Assert.True(exists);
            Assert.Equal("Khoa đã tồn tại!", message);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAndSaveDepartment()
        {
            // Arrange
            var newDepartment = new Department { Name = "Khoa Công nghệ thông tin" };

            _mockDepartmentRepository.Setup(repo => repo.Add(newDepartment));
            _mockDepartmentRepository.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _departmentService.CreateAsync(newDepartment);

            // Assert
            Assert.Equal(newDepartment, result);
            _mockDepartmentRepository.Verify(repo => repo.Add(newDepartment), Times.Once);
            _mockDepartmentRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldSucceed()
        {
            // Arrange
            var departmentId = 1;
            var existingDepartment = new Department { Id = departmentId, Name = "Khoa Công nghệ thông tin" };
            var updatedDepartment = new Department { Id = departmentId, Name = "Khoa CNTT" };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync(existingDepartment);
            _mockDepartmentRepository.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _departmentService.UpdateAsync(departmentId, updatedDepartment);

            // Assert
            Assert.True(result);
            Assert.Equal(updatedDepartment.Name, existingDepartment.Name);
            _mockDepartmentRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistentDepartment_ShouldFail()
        {
            // Arrange
            var departmentId = 999;
            var updatedDepartment = new Department { Id = departmentId, Name = "Khoa CNTT" };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync((Department)null);

            // Act
            var result = await _departmentService.UpdateAsync(departmentId, updatedDepartment);

            // Assert
            Assert.False(result);
            _mockDepartmentRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingDepartment_ShouldSucceed()
        {
            // Arrange
            var departmentId = 1;
            var department = new Department { Id = departmentId, Name = "Khoa Công nghệ thông tin" };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync(department);
            _mockDepartmentRepository.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _departmentService.DeleteAsync(departmentId);

            // Assert
            Assert.True(result);
            _mockDepartmentRepository.Verify(repo => repo.Remove(department), Times.Once);
            _mockDepartmentRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistentDepartment_ShouldFail()
        {
            // Arrange
            var departmentId = 999;
            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync((Department)null);

            // Act
            var result = await _departmentService.DeleteAsync(departmentId);

            // Assert
            Assert.False(result);
            _mockDepartmentRepository.Verify(repo => repo.Remove(It.IsAny<Department>()), Times.Never);
            _mockDepartmentRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
} 