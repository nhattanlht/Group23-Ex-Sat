using Xunit;
using Moq;
using StudentManagement.Services;
using StudentManagement.Repositories;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Services
{
    public class ProgramServiceTests
    {
        private readonly Mock<IProgramRepository> _mockProgramRepository;
        private readonly IProgramService _programService;

        public ProgramServiceTests()
        {
            _mockProgramRepository = new Mock<IProgramRepository>();
            _programService = new ProgramService(_mockProgramRepository.Object);
        }

        [Fact]
        public async Task GetAllProgramsAsync_ShouldReturnAllPrograms()
        {
            // Arrange
            var expectedPrograms = new List<StudyProgram>
            {
                new StudyProgram { Id = 1, Name = "Công nghệ thông tin" },
                new StudyProgram { Id = 2, Name = "Kỹ thuật điện" }
            };

            _mockProgramRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedPrograms);

            // Act
            var result = await _programService.GetAllProgramsAsync();

            // Assert
            Assert.Equal(expectedPrograms, result);
        }

        [Fact]
        public async Task GetProgramByIdAsync_ExistingProgram_ShouldReturnProgram()
        {
            // Arrange
            var programId = 1;
            var expectedProgram = new StudyProgram { Id = programId, Name = "Công nghệ thông tin" };

            _mockProgramRepository.Setup(repo => repo.GetByIdAsync(programId))
                .ReturnsAsync(expectedProgram);

            // Act
            var result = await _programService.GetProgramByIdAsync(programId);

            // Assert
            Assert.Equal(expectedProgram, result);
        }

        [Fact]
        public async Task GetProgramByIdAsync_NonExistentProgram_ShouldReturnNull()
        {
            // Arrange
            var programId = 999;
            _mockProgramRepository.Setup(repo => repo.GetByIdAsync(programId))
                .ReturnsAsync((StudyProgram)null);

            // Act
            var result = await _programService.GetProgramByIdAsync(programId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProgramAsync_WithNewName_ShouldSucceed()
        {
            // Arrange
            var newProgram = new StudyProgram { Name = "Công nghệ thông tin" };

            _mockProgramRepository.Setup(repo => repo.ExistsByNameAsync(newProgram.Name))
                .ReturnsAsync(false);
            _mockProgramRepository.Setup(repo => repo.AddAsync(newProgram))
                .ReturnsAsync(newProgram);

            // Act
            var result = await _programService.CreateProgramAsync(newProgram);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newProgram.Name, result.Name);
        }

        [Fact]
        public async Task CreateProgramAsync_WithExistingName_ShouldReturnNull()
        {
            // Arrange
            var newProgram = new StudyProgram { Name = "Công nghệ thông tin" };

            _mockProgramRepository.Setup(repo => repo.ExistsByNameAsync(newProgram.Name))
                .ReturnsAsync(true);

            // Act
            var result = await _programService.CreateProgramAsync(newProgram);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProgramAsync_WithValidData_ShouldSucceed()
        {
            // Arrange
            var programId = 1;
            var updatedProgram = new StudyProgram { Id = programId, Name = "Công nghệ thông tin mới" };

            _mockProgramRepository.Setup(repo => repo.UpdateAsync(updatedProgram))
                .ReturnsAsync(true);

            // Act
            var result = await _programService.UpdateProgramAsync(programId, updatedProgram);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateProgramAsync_WithMismatchedId_ShouldFail()
        {
            // Arrange
            var programId = 1;
            var updatedProgram = new StudyProgram { Id = 2, Name = "Công nghệ thông tin mới" };

            // Act
            var result = await _programService.UpdateProgramAsync(programId, updatedProgram);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateProgramAsync_NonExistentProgram_ShouldFail()
        {
            // Arrange
            var programId = 1;
            var updatedProgram = new StudyProgram { Id = programId, Name = "Công nghệ thông tin mới" };

            _mockProgramRepository.Setup(repo => repo.UpdateAsync(updatedProgram))
                .ReturnsAsync(false);

            // Act
            var result = await _programService.UpdateProgramAsync(programId, updatedProgram);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteProgramAsync_ExistingProgramWithoutStudents_ShouldSucceed()
        {
            // Arrange
            var programId = 1;
            _mockProgramRepository.Setup(repo => repo.DeleteAsync(programId))
                .ReturnsAsync(true);

            // Act
            var result = await _programService.DeleteProgramAsync(programId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProgramAsync_NonExistentProgram_ShouldFail()
        {
            // Arrange
            var programId = 999;
            _mockProgramRepository.Setup(repo => repo.DeleteAsync(programId))
                .ReturnsAsync(false);

            // Act
            var result = await _programService.DeleteProgramAsync(programId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteProgramAsync_ProgramWithStudents_ShouldFail()
        {
            // Arrange
            var programId = 1;
            _mockProgramRepository.Setup(repo => repo.DeleteAsync(programId))
                .ReturnsAsync(false);

            // Act
            var result = await _programService.DeleteProgramAsync(programId);

            // Assert
            Assert.False(result);
        }
    }
} 