using Xunit;
using Moq;
using StudentManagement.Services;
using StudentManagement.Repositories;
using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Services
{
    public class AddressServiceTests
    {
        private readonly Mock<IAddressRepository> _mockRepository;
        private readonly IAddressService _service;

        public AddressServiceTests()
        {
            _mockRepository = new Mock<IAddressRepository>();
            _service = new AddressService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAddressByIdAsync_ExistingAddress_ShouldReturnAddress()
        {
            // Arrange
            var addressId = 1;
            var expectedAddress = new Address
            {
                Id = addressId,
                HouseNumber = "123",
                StreetName = "Nguyễn Văn Linh",
                Ward = "Phường 7",
                District = "Quận 8",
                Province = "TP.HCM",
                Country = "Việt Nam"
            };

            _mockRepository.Setup(repo => repo.GetAddressByIdAsync(addressId))
                .ReturnsAsync(expectedAddress);

            // Act
            var result = await _service.GetAddressByIdAsync(addressId);

            // Assert
            Assert.Equal(expectedAddress, result);
            _mockRepository.Verify(repo => repo.GetAddressByIdAsync(addressId), Times.Once);
        }

        [Fact]
        public async Task GetAddressByIdAsync_NonExistentAddress_ShouldReturnNull()
        {
            // Arrange
            var addressId = 999;
            _mockRepository.Setup(repo => repo.GetAddressByIdAsync(addressId))
                .ReturnsAsync((Address)null);

            // Act
            var result = await _service.GetAddressByIdAsync(addressId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetAddressByIdAsync(addressId), Times.Once);
        }

        [Fact]
        public async Task CreateAddressAsync_WithValidData_ShouldCreateAddress()
        {
            // Arrange
            var newAddress = new Address
            {
                HouseNumber = "456",
                StreetName = "Lê Văn Việt",
                Ward = "Phường Tăng Nhơn Phú A",
                District = "Quận 9",
                Province = "TP.HCM",
                Country = "Việt Nam"
            };

            _mockRepository.Setup(repo => repo.AddAddressAsync(newAddress))
                .ReturnsAsync(newAddress);

            // Act
            var result = await _service.CreateAddressAsync(newAddress);

            // Assert
            Assert.Equal(newAddress, result);
            _mockRepository.Verify(repo => repo.AddAddressAsync(newAddress), Times.Once);
        }

        [Fact]
        public async Task CreateAddressAsync_WithEmptyFields_ShouldCreateAddress()
        {
            // Arrange
            var newAddress = new Address
            {
                HouseNumber = "",
                StreetName = "",
                Ward = "",
                District = "",
                Province = "",
                Country = ""
            };

            _mockRepository.Setup(repo => repo.AddAddressAsync(newAddress))
                .ReturnsAsync(newAddress);

            // Act
            var result = await _service.CreateAddressAsync(newAddress);

            // Assert
            Assert.Equal(newAddress, result);
            _mockRepository.Verify(repo => repo.AddAddressAsync(newAddress), Times.Once);
        }
    }
} 