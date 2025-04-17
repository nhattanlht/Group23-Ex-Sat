using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class EnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetAllAsync() =>
            await _context.Enrollments
                          .Include(e => e.Student)
                          .Include(e => e.Class)
                          .ToListAsync();

        public async Task<Enrollment?> GetByIdAsync(int enrollmentId) =>
            await _context.Enrollments
                          .Include(e => e.Student)
                          .Include(e => e.Class)
                          .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

        public async Task AddAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int enrollmentId)
        {
            var existing = await GetByIdAsync(enrollmentId);
            if (existing != null)
            {
                _context.Enrollments.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}