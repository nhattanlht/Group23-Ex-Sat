using Moq;
using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services
{
    public class GradeServiceTests
    {
        private readonly Mock<IGradeRepository> _mockClassRepository;
        private readonly GradeService _classService;

        public GradeServiceTests()
        {
            _mockClassRepository = new Mock<IGradeRepository>();
            _classService = new GradeService(_mockClassRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGrades()
        {
            // Arrange
            var expectedGrades = new List<Grade>
            {
                new Grade { GradeId = 1, StudentId = "S1", ClassId = "C1", Score = 9.0 },
                new Grade { GradeId = 2, StudentId = "S2", ClassId = "C2", Score = 8.5 }
            };
            _mockClassRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedGrades);

            // Act
            var result = await _classService.GetAllAsync();

            // Assert
            Assert.Equal(expectedGrades, result);
            _mockClassRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenGradeExists_ShouldReturnGrade()
        {
            // Arrange
            var studentId = "S1";
            var classId = "C1";
            var expectedGrade = new Grade { GradeId = 1, StudentId = studentId, ClassId = classId, Score = 9.0 };
            _mockClassRepository.Setup(repo => repo.GetByIdAsync(studentId, classId))
                .ReturnsAsync(expectedGrade);

            // Act
            var result = await _classService.GetByIdAsync(studentId, classId);

            // Assert
            Assert.Equal(expectedGrade, result);
            _mockClassRepository.Verify(repo => repo.GetByIdAsync(studentId, classId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenGradeDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var studentId = "S1";
            var classId = "C1";
            var expectedGrade = (Grade)null;
            _mockClassRepository.Setup(repo => repo.GetByIdAsync(studentId, classId))
                .ReturnsAsync(expectedGrade);

            // Act
            var result = await _classService.GetByIdAsync(studentId, classId);

            // Assert
            Assert.Null(result);
            _mockClassRepository.Verify(repo => repo.GetByIdAsync(studentId, classId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var newGrade = new Grade { GradeId = 1, StudentId = "S1", ClassId = "C1", Score = 9.0 };
            _mockClassRepository.Setup(repo => repo.AddAsync(newGrade))
                .Returns(Task.CompletedTask);

            // Act
            await _classService.AddAsync(newGrade);

            // Assert
            _mockClassRepository.Verify(repo => repo.AddAsync(newGrade), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var existingGrade = new Grade { GradeId = 1, StudentId = "S1", ClassId = "C1", Score = 9.0 };
            _mockClassRepository.Setup(repo => repo.UpdateAsync(existingGrade))
                .Returns(Task.CompletedTask);

            // Act
            await _classService.UpdateAsync(existingGrade);

            // Assert
            _mockClassRepository.Verify(repo => repo.UpdateAsync(existingGrade), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            var gradeId = 1;
            _mockClassRepository.Setup(repo => repo.DeleteAsync(gradeId))
                .Returns(Task.CompletedTask);

            // Act
            await _classService.DeleteAsync(gradeId);

            // Assert
            _mockClassRepository.Verify(repo => repo.DeleteAsync(gradeId), Times.Once);
        }
    }
}