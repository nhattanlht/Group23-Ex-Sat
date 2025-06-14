using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class IdentificationRepository : IIdentificationRepository
    {
        private readonly ApplicationDbContext _context;

        public IdentificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Identification?> GetByIdAsync(int id)
        {
            return await _context.Identifications.FindAsync(id);
        }

        public async Task<Identification> AddAsync(Identification identification)
        {
            _context.Identifications.Add(identification);
            await _context.SaveChangesAsync();
            return identification;
        }
    }
}