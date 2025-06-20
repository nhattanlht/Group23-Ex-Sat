using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Grade>> GetAllAsync() =>
            await _context.Grades
                          .Include(g => g.Student)
                          .Include(g => g.Class)
                          .ThenInclude(c => c.Course)
                          .ToListAsync();

        public async Task<Grade?> GetByIdAsync(string StudentId, string classId) =>
            await _context.Grades
                          .Include(g => g.Student)
                          .Include(g => g.Class)
                          .ThenInclude(c => c.Course)
                          .FirstOrDefaultAsync(g => g.StudentId == StudentId && g.ClassId == classId);

        public async Task AddAsync(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Grade grade)
        {
            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int gradeId)
        {
            var grade = await _context.Grades.FindAsync(gradeId);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> HasGradeForClassAsync(string classId)
        {
            return await _context.Grades.AnyAsync(g => g.ClassId == classId);
        }
    }

}