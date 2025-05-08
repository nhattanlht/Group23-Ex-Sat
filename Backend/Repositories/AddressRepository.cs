using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<Address> AddAddressAsync(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}