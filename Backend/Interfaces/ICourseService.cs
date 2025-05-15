using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface ICourseService
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Course>> GetActiveCoursesAsync();
        Task<Course?> GetCourseByCodeAsync(string code);
        Task<bool> CreateCourseAsync(Course course);
        Task<bool> HasStudentRegistrationsAsync(string courseCode);
        Task<bool> UpdateCourseAsync(Course updatedCourse);
        Task<bool> DeleteCourseAsync(string courseCode);
    }
}