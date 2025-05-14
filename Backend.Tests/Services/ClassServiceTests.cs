using Moq;
using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services
{
    public class ClassServiceTests
    {
        private readonly Mock<IClassRepository> _mockRepository;
        private readonly ClassService _service;

        public ClassServiceTests()
        {
            _mockRepository = new Mock<IClassRepository>();
            _service = new ClassService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllClasses()
        {
            // Arrange
            var expectedClasses = new List<Class>
            {
                new Class { ClassId = "CS101", CourseCode = "CS101", Teacher = "John Doe" },
                new Class { ClassId = "CS102", CourseCode = "CS102", Teacher = "Jane Smith" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedClasses);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(expectedClasses, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenClassExists_ShouldReturnClass()
        {
            // Arrange
            var classId = "CS101";
            var expectedClass = new Class { ClassId = classId, CourseCode = "CS101", Teacher = "John Doe" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(classId))
                .ReturnsAsync(expectedClass);

            // Act
            var result = await _service.GetByIdAsync(classId);

            // Assert
            Assert.Equal(expectedClass, result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(classId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenClassDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var classId = "CS999";
            _mockRepository.Setup(repo => repo.GetByIdAsync(classId))
                .ReturnsAsync((Class?)null);

            // Act
            var result = await _service.GetByIdAsync(classId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(classId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var newClass = new Class { ClassId = "CS101", CourseCode = "CS101", Teacher = "John Doe" };
            _mockRepository.Setup(repo => repo.AddAsync(newClass))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddAsync(newClass);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(newClass), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var existingClass = new Class { ClassId = "CS101", CourseCode = "CS101", Teacher = "John Doe" };
            _mockRepository.Setup(repo => repo.UpdateAsync(existingClass))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(existingClass);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(existingClass), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            var classId = "CS101";
            _mockRepository.Setup(repo => repo.DeleteAsync(classId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(classId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(classId), Times.Once);
        }
    }
} 