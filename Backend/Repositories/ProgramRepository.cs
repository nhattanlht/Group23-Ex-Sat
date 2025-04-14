using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class ProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<StudyProgram>> GetAllAsync() =>
            _context.StudyPrograms.ToListAsync();

        public Task<StudyProgram?> GetByIdAsync(int id) =>
            _context.StudyPrograms.FindAsync(id).AsTask();

        public Task<bool> ExistsByNameAsync(string name) =>
            _context.StudyPrograms.AnyAsync(p => p.Name == name);

        public async Task<StudyProgram> AddAsync(StudyProgram program)
        {
            _context.StudyPrograms.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<bool> UpdateAsync(StudyProgram updated)
        {
            var existing = await _context.StudyPrograms.FindAsync(updated.Id);
            if (existing == null) return false;

            existing.Name = updated.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var program = await _context.StudyPrograms.Include(p => p.Students).FirstOrDefaultAsync(p => p.Id == id);
            if (program == null || program.Students.Any()) return false;

            _context.StudyPrograms.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}