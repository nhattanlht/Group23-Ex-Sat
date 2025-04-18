using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class CourseService
    {
        private readonly CourseRepository _repository;

        public CourseService(CourseRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Course?> GetCourseByCodeAsync(string code)
        {
            return await _repository.GetByCodeAsync(code);
        }

        public async Task<bool> CreateCourseAsync(Course course)
        {
            if (course.Credits < 2) return false;

            var prereqExists = string.IsNullOrEmpty(course.PrerequisiteCourseCode) ||
                               await _repository.GetByCodeAsync(course.PrerequisiteCourseCode) != null;

            if (!prereqExists) return false;

            await _repository.AddAsync(course);
            return true;
        }

        public async Task<bool> UpdateCourseAsync(Course updatedCourse)
        {
            var course = await _repository.GetByCodeAsync(updatedCourse.CourseCode);
            if (course == null) return false;

            course.Name = updatedCourse.Name;
            course.Description = updatedCourse.Description;
            course.DepartmentId = updatedCourse.DepartmentId;

            await _repository.UpdateAsync(course);
            return true;
        }
        public async Task<bool> DeleteCourseAsync(string courseCode)
        {
            var course = await _repository.GetByCodeAsync(courseCode);
            if (course == null) return false;

            var hasOpenClasses = await _repository.HasOpenClassesAsync(courseCode);
            if (hasOpenClasses)
            {
                course.IsActive = false;
                await _repository.UpdateAsync(course);
                return true;
            }

            await _repository.DeleteAsync(course);
            return true;
        }
    }
}
