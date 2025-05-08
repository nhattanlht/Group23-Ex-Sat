using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;

        public AddressService(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            return await _repository.GetAddressByIdAsync(id);
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            return await _repository.AddAddressAsync(address);
        }
    }
}