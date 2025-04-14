using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class SchoolYearRepository
    {
        private readonly ApplicationDbContext _context;

        public SchoolYearRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<SchoolYear>> GetAllAsync() =>
            _context.SchoolYears.ToListAsync();

        public Task<SchoolYear?> GetByIdAsync(int id) =>
            _context.SchoolYears.FindAsync(id).AsTask();

        public async Task<SchoolYear> AddAsync(SchoolYear schoolYear)
        {
            _context.SchoolYears.Add(schoolYear);
            await _context.SaveChangesAsync();
            return schoolYear;
        }

        public async Task<bool> UpdateAsync(SchoolYear updated)
        {
            var existing = await _context.SchoolYears.FindAsync(updated.Id);
            if (existing == null) return false;

            existing.Name = updated.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var schoolYear = await _context.SchoolYears.FindAsync(id);
            if (schoolYear == null) return false;

            _context.SchoolYears.Remove(schoolYear);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}