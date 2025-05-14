using Xunit;
using Moq;
using StudentManagement.Services;
using StudentManagement.Repositories;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Services
{
    public class SchoolYearServiceTests
    {
        private readonly Mock<ISchoolYearRepository> _mockRepository;
        private readonly ISchoolYearService _service;

        public SchoolYearServiceTests()
        {
            _mockRepository = new Mock<ISchoolYearRepository>();
            _service = new SchoolYearService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllSchoolYearsAsync_ShouldReturnAllSchoolYears()
        {
            // Arrange
            var expectedSchoolYears = new List<SchoolYear>
            {
                new SchoolYear { Id = 1, Name = "2020-2021" },
                new SchoolYear { Id = 2, Name = "2021-2022" },
                new SchoolYear { Id = 3, Name = "2022-2023" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedSchoolYears);

            // Act
            var result = await _service.GetAllSchoolYearsAsync();

            // Assert
            Assert.Equal(expectedSchoolYears, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSchoolYearByIdAsync_ExistingSchoolYear_ShouldReturnSchoolYear()
        {
            // Arrange
            var schoolYearId = 1;
            var expectedSchoolYear = new SchoolYear { Id = schoolYearId, Name = "2020-2021" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(schoolYearId))
                .ReturnsAsync(expectedSchoolYear);

            // Act
            var result = await _service.GetSchoolYearByIdAsync(schoolYearId);

            // Assert
            Assert.Equal(expectedSchoolYear, result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(schoolYearId), Times.Once);
        }

        [Fact]
        public async Task GetSchoolYearByIdAsync_NonExistentSchoolYear_ShouldReturnNull()
        {
            // Arrange
            var schoolYearId = 999;
            _mockRepository.Setup(repo => repo.GetByIdAsync(schoolYearId))
                .ReturnsAsync((SchoolYear)null);

            // Act
            var result = await _service.GetSchoolYearByIdAsync(schoolYearId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(schoolYearId), Times.Once);
        }

        [Fact]
        public async Task CreateSchoolYearAsync_WithValidData_ShouldCreateSchoolYear()
        {
            // Arrange
            var newSchoolYear = new SchoolYear { Name = "2023-2024" };
            _mockRepository.Setup(repo => repo.AddAsync(newSchoolYear))
                .ReturnsAsync(newSchoolYear);

            // Act
            var result = await _service.CreateSchoolYearAsync(newSchoolYear);

            // Assert
            Assert.Equal(newSchoolYear, result);
            _mockRepository.Verify(repo => repo.AddAsync(newSchoolYear), Times.Once);
        }

        [Fact]
        public async Task CreateSchoolYearAsync_WithEmptyName_ShouldReturnNull()
        {
            // Arrange
            var newSchoolYear = new SchoolYear { Name = "" };

            // Act
            var result = await _service.CreateSchoolYearAsync(newSchoolYear);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<SchoolYear>()), Times.Never);
        }

        [Fact]
        public async Task CreateSchoolYearAsync_WithWhitespaceName_ShouldReturnNull()
        {
            // Arrange
            var newSchoolYear = new SchoolYear { Name = "   " };

            // Act
            var result = await _service.CreateSchoolYearAsync(newSchoolYear);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<SchoolYear>()), Times.Never);
        }

        [Fact]
        public async Task UpdateSchoolYearAsync_WithValidData_ShouldSucceed()
        {
            // Arrange
            var schoolYearId = 1;
            var updatedSchoolYear = new SchoolYear { Id = schoolYearId, Name = "2020-2021 (Updated)" };
            _mockRepository.Setup(repo => repo.UpdateAsync(updatedSchoolYear))
                .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateSchoolYearAsync(schoolYearId, updatedSchoolYear);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(updatedSchoolYear), Times.Once);
        }

        [Fact]
        public async Task UpdateSchoolYearAsync_WithMismatchedId_ShouldFail()
        {
            // Arrange
            var schoolYearId = 1;
            var updatedSchoolYear = new SchoolYear { Id = 2, Name = "2020-2021 (Updated)" };

            // Act
            var result = await _service.UpdateSchoolYearAsync(schoolYearId, updatedSchoolYear);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<SchoolYear>()), Times.Never);
        }

        [Fact]
        public async Task DeleteSchoolYearAsync_ExistingSchoolYear_ShouldSucceed()
        {
            // Arrange
            var schoolYearId = 1;
            _mockRepository.Setup(repo => repo.DeleteAsync(schoolYearId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteSchoolYearAsync(schoolYearId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(schoolYearId), Times.Once);
        }

        [Fact]
        public async Task DeleteSchoolYearAsync_NonExistentSchoolYear_ShouldFail()
        {
            // Arrange
            var schoolYearId = 999;
            _mockRepository.Setup(repo => repo.DeleteAsync(schoolYearId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteSchoolYearAsync(schoolYearId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(schoolYearId), Times.Once);
        }
    }
} 