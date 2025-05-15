using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IAddressService
    {
        Task<Address?> GetAddressByIdAsync(int id);
        Task<Address> CreateAddressAsync(Address address);
    }
} 