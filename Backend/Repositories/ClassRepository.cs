using StudentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Repositories
{
    public class ClassRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllAsync() =>
            await _context.Classes.Include(c => c.Course).ToListAsync();

        public async Task<Class?> GetByIdAsync(string classCode) =>
            await _context.Classes.Include(c => c.Course)
                                  .FirstOrDefaultAsync(c => c.ClassId == classCode);

        public async Task AddAsync(Class classEntity)
        {
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class classEntity)
        {
            _context.Classes.Update(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string classCode)
        {
            var existing = await GetByIdAsync(classCode);
            if (existing != null)
            {
                _context.Classes.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}