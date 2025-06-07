using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
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

        public async Task<bool> HasPrerequisiteAsync(string StudentId, string classId)
        {
            var classInfo = await _context.Classes
                                   .Include(c => c.Course)
                                   .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classInfo == null) return false; 

            var prerequisiteCourseCode = classInfo.Course.PrerequisiteCourseCode;

            if (string.IsNullOrEmpty(prerequisiteCourseCode)) return true;

            var hasCompletedPrerequisite = await _context.Enrollments
                                                        .Include(e => e.Class)
                                                        .Where(e => e.StudentId == StudentId && e.Class.CourseCode == prerequisiteCourseCode)
                                                        .AnyAsync();

            return hasCompletedPrerequisite;
        }

        public async Task<bool> IsClassFullAsync(string classId)
        {
            var classInfo = await _context.Classes
                                   .Include(c => c.Enrollments)
                                   .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classInfo == null) return false; 
            var numStudent = await _context.Enrollments.CountAsync(e => e.ClassId == classId);

            return numStudent >= classInfo.MaxStudents;
        }

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