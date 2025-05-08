using Moq;
using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services
{
    public class IdentificationServiceTests
    {
        private readonly Mock<IIdentificationRepository> _mockRepository;
        private readonly IdentificationService _service;

        public IdentificationServiceTests()
        {
            _mockRepository = new Mock<IIdentificationRepository>();
            _service = new IdentificationService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateIdentificationAsync_ShouldReturnCreatedIdentification()
        {
            // Arrange
            var identification = new Identification
            {
                IdentificationType = "CCCD",
                Number = "123456789",
                IssueDate = DateTime.Now,
                IssuedBy = "Công an TP.HCM",
                HasChip = true
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Identification>()))
                .ReturnsAsync(identification);

            // Act
            var result = await _service.CreateIdentificationAsync(identification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(identification.IdentificationType, result.IdentificationType);
            Assert.Equal(identification.Number, result.Number);
            Assert.Equal(identification.IssuedBy, result.IssuedBy);
            Assert.Equal(identification.HasChip, result.HasChip);
            _mockRepository.Verify(repo => repo.AddAsync(identification), Times.Once);
        }

        [Fact]
        public async Task GetIdentificationByIdAsync_WhenIdentificationExists_ShouldReturnIdentification()
        {
            // Arrange
            var expectedIdentification = new Identification
            {
                Id = 1,
                IdentificationType = "CCCD",
                Number = "123456789",
                IssueDate = DateTime.Now,
                IssuedBy = "Công an TP.HCM",
                HasChip = true
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedIdentification);

            // Act
            var result = await _service.GetIdentificationByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedIdentification.Id, result.Id);
            Assert.Equal(expectedIdentification.IdentificationType, result.IdentificationType);
            Assert.Equal(expectedIdentification.Number, result.Number);
            Assert.Equal(expectedIdentification.IssuedBy, result.IssuedBy);
            Assert.Equal(expectedIdentification.HasChip, result.HasChip);
            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetIdentificationByIdAsync_WhenIdentificationDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Identification)null);

            // Act
            var result = await _service.GetIdentificationByIdAsync(999);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CreateIdentificationAsync_WithPassport_ShouldHandlePassportSpecificFields()
        {
            // Arrange
            var identification = new Identification
            {
                IdentificationType = "Passport",
                Number = "P123456789",
                IssueDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(10),
                IssuedBy = "Cục Quản lý xuất nhập cảnh",
                IssuingCountry = "Việt Nam"
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Identification>()))
                .ReturnsAsync(identification);

            // Act
            var result = await _service.CreateIdentificationAsync(identification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Passport", result.IdentificationType);
            Assert.Equal("P123456789", result.Number);
            Assert.Equal("Cục Quản lý xuất nhập cảnh", result.IssuedBy);
            Assert.Equal("Việt Nam", result.IssuingCountry);
            Assert.NotNull(result.ExpiryDate);
            _mockRepository.Verify(repo => repo.AddAsync(identification), Times.Once);
        }

        [Fact]
        public async Task CreateIdentificationAsync_WithCMND_ShouldHandleCMNDSpecificFields()
        {
            // Arrange
            var identification = new Identification
            {
                IdentificationType = "CMND",
                Number = "123456789",
                IssueDate = DateTime.Now,
                IssuedBy = "Công an TP.HCM",
                Notes = "CMND cũ"
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Identification>()))
                .ReturnsAsync(identification);

            // Act
            var result = await _service.CreateIdentificationAsync(identification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CMND", result.IdentificationType);
            Assert.Equal("123456789", result.Number);
            Assert.Equal("Công an TP.HCM", result.IssuedBy);
            Assert.Equal("CMND cũ", result.Notes);
            Assert.Null(result.HasChip);
            Assert.Null(result.IssuingCountry);
            _mockRepository.Verify(repo => repo.AddAsync(identification), Times.Once);
        }
    }
} 