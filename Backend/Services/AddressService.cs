using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class AddressService
    {
        private readonly AddressRepository _repository;

        public AddressService(AddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _repository.GetAddressByIdAsync(id);
        }

        public async Task CreateAddressAsync(Address address)
        {
            await _repository.AddAddressAsync(address);
        }
    }
}