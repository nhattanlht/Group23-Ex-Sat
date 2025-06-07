using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using StudentManagement.Services;
using StudentManagement.Repositories;
using StudentManagement.Models;

namespace StudentManagement.Tests.Services
{
    public class GradeServiceTest
    {
        private readonly Mock<IGradeRepository> _gradeRepositoryMock;
        private readonly GradeService _gradeService;

        public GradeServiceTest()
        {
            _gradeRepositoryMock = new Mock<IGradeRepository>();
            _gradeService = new GradeService(_gradeRepositoryMock.Object);
        }

        [Fact]
        public async Task GetGradesByStudentIdAsync_ShouldReturnGrades_WhenStudentExists()
        {
            // Arrange
            var StudentId = "001";
            var classId = "CS101";
            var grades = new Grade { GradeId = 1, StudentId = StudentId, ClassId = "CS101", Score = 8.5 };

            _gradeRepositoryMock.Setup(repo => repo.GetByIdAsync(StudentId, classId))
                                .ReturnsAsync(grades);

            // Act
            var result = await _gradeService.GetByIdAsync(StudentId, classId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CS101", result.ClassId);
            Assert.Equal(8.5, result.Score);
        }

        [Fact]
        public async Task GetGradesByStudentIdAsync_ShouldReturnEmptyList_WhenStudentDoesNotExist()
        {
            // Arrange
            var StudentId = "12345";
            var classId = "CS101";
            _gradeRepositoryMock.Setup(repo => repo.GetByIdAsync(StudentId, classId))
                                .ReturnsAsync(new Grade());

            // Act
            var result = await _gradeService.GetByIdAsync(StudentId, classId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddGradeAsync_ShouldAddGrade_WhenValidGradeIsProvided()
        {
            // Arrange
            var grade = new Grade { GradeId = 1, StudentId = "001", ClassId = "CS101", Score = 9.5 };
            _gradeRepositoryMock.Setup(repo => repo.AddAsync(grade))
                                .Returns(Task.CompletedTask);

            // Act
            await _gradeService.AddAsync(grade);

            // Assert
            _gradeRepositoryMock.Verify(repo => repo.AddAsync(grade), Times.Once);
        }

        [Fact]
        public async Task DeleteGradeAsync_ShouldDeleteGrade_WhenGradeExists()
        {
            // Arrange
            var gradeId = 1;
            _gradeRepositoryMock.Setup(repo => repo.DeleteAsync(gradeId))
                                .Returns(Task.CompletedTask);

            // Act
            await _gradeService.DeleteAsync(gradeId);

            // Assert
            _gradeRepositoryMock.Verify(repo => repo.DeleteAsync(gradeId), Times.Once);
        }
    }
}