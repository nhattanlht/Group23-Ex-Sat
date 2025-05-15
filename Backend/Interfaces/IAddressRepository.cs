using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IAddressRepository
    {
        Task<Address?> GetAddressByIdAsync(int id);
        Task<Address> AddAddressAsync(Address address);
    }
} 