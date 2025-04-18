using StudentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Repositories
{
    public class CourseRepository:ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.Include(c => c.Department).ToListAsync();
        }

        public async Task<List<Course>> GetActiveCoursesAsync()
        {
            return await _context.Courses.Include(c => c.Department).Where(c => c.IsActive).ToListAsync();
        }

        public async Task<bool> HasStudentRegistrationsAsync(string courseCode)
        {
            var hasEnrollments = await _context.Enrollments
                .Include(e => e.Class)
                .Where(e => e.Class.CourseCode == courseCode) 
                .AnyAsync(); 

            return hasEnrollments;
        }

        public async Task<Course?> GetByCodeAsync(string code)
        {
            return await _context.Courses.Include(c => c.Department).FirstOrDefaultAsync(c => c.CourseCode == code);
        }

        public async Task<bool> HasOpenClassesAsync(string courseCode)
        {
            return await _context.Classes
                .AnyAsync(c => c.CourseCode == courseCode);
        }

        public async Task AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
