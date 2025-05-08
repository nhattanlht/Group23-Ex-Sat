using Moq;
using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services
{
    public class EnrollmentServiceTests
    {
        private readonly Mock<IEnrollmentRepository> _mockRepository;
        private readonly EnrollmentService _service;

        public EnrollmentServiceTests()
        {
            _mockRepository = new Mock<IEnrollmentRepository>();
            _service = new EnrollmentService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEnrollments()
        {
            // Arrange
            var expectedEnrollments = new List<Enrollment>
            {
                new Enrollment { EnrollmentId = 1, StudentId = "SV001", ClassId = "C001" },
                new Enrollment { EnrollmentId = 2, StudentId = "SV002", ClassId = "C002" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedEnrollments);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(expectedEnrollments, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenEnrollmentExists_ShouldReturnEnrollment()
        {
            // Arrange
            var expectedEnrollment = new Enrollment 
            { 
                EnrollmentId = 1, 
                StudentId = "SV001", 
                ClassId = "C001" 
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedEnrollment);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.Equal(expectedEnrollment, result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenEnrollmentDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Enrollment)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task HasPrerequisiteAsync_WhenPrerequisiteExists_ShouldReturnTrue()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.HasPrerequisiteAsync("SV001", "C001"))
                .ReturnsAsync(true);

            // Act
            var result = await _service.HasPrerequisiteAsync("SV001", "C001");

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.HasPrerequisiteAsync("SV001", "C001"), Times.Once);
        }

        [Fact]
        public async Task IsClassFullAsync_WhenClassIsFull_ShouldReturnTrue()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.IsClassFullAsync("C001"))
                .ReturnsAsync(true);

            // Act
            var result = await _service.IsClassFullAsync("C001");

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.IsClassFullAsync("C001"), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var enrollment = new Enrollment 
            { 
                StudentId = "SV001", 
                ClassId = "C001",
                RegisteredAt = DateTime.Now
            };

            // Act
            await _service.AddAsync(enrollment);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(enrollment), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var enrollment = new Enrollment 
            { 
                EnrollmentId = 1,
                StudentId = "SV001", 
                ClassId = "C001"
            };

            // Act
            await _service.UpdateAsync(enrollment);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(enrollment), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            var enrollmentId = 1;

            // Act
            await _service.DeleteAsync(enrollmentId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(enrollmentId), Times.Once);
        }

        [Fact]
        public async Task CancelEnrollmentAsync_WhenEnrollmentExistsAndNotCancelled_ShouldCancelEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment 
            { 
                EnrollmentId = 1,
                StudentId = "SV001", 
                ClassId = "C001",
                IsCancelled = false,
                Class = new Class 
                { 
                    CancelDeadline = DateTime.Now.AddDays(1) 
                }
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(enrollment);

            // Act
            var result = await _service.CancelEnrollmentAsync(1, "Test reason");

            // Assert
            Assert.True(result);
            Assert.True(enrollment.IsCancelled);
            Assert.Equal("Test reason", enrollment.CancelReason);
            Assert.NotNull(enrollment.CancelDate);
            _mockRepository.Verify(repo => repo.UpdateAsync(enrollment), Times.Once);
        }

        [Fact]
        public async Task CancelEnrollmentAsync_WhenEnrollmentDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Enrollment)null);

            // Act
            var result = await _service.CancelEnrollmentAsync(999, "Test reason");

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enrollment>()), Times.Never);
        }

        [Fact]
        public async Task CancelEnrollmentAsync_WhenEnrollmentAlreadyCancelled_ShouldReturnFalse()
        {
            // Arrange
            var enrollment = new Enrollment 
            { 
                EnrollmentId = 1,
                IsCancelled = true
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(enrollment);

            // Act
            var result = await _service.CancelEnrollmentAsync(1, "Test reason");

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enrollment>()), Times.Never);
        }

        [Fact]
        public async Task CancelEnrollmentAsync_WhenPastDeadline_ShouldReturnFalse()
        {
            // Arrange
            var enrollment = new Enrollment 
            { 
                EnrollmentId = 1,
                IsCancelled = false,
                Class = new Class 
                { 
                    CancelDeadline = DateTime.Now.AddDays(-1) 
                }
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(enrollment);

            // Act
            var result = await _service.CancelEnrollmentAsync(1, "Test reason");

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enrollment>()), Times.Never);
        }
    }
} 