using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<List<Course>> GetActiveCoursesAsync();
        Task<bool> HasStudentRegistrationsAsync(string courseCode);
        Task<Course?> GetByCodeAsync(string code);
        Task<bool> HasOpenClassesAsync(string courseCode);
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(Course course);
    }
}