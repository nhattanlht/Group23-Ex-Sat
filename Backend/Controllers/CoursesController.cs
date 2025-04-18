using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _service.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCourses()
        {
            var courses = await _service.GetActiveCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var course = await _service.GetCourseByCodeAsync(code);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PrerequisiteCourseCode))
            {
                dto.PrerequisiteCourseCode = null;
            }
            
            var course = new Course
            {
                CourseCode = dto.CourseCode,
                Name = dto.Name,
                Description = dto.Description,
                Credits = dto.Credits,
                PrerequisiteCourseCode = dto.PrerequisiteCourseCode,
                DepartmentId = dto.DepartmentId,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt
            };

            await _service.CreateCourseAsync(course);
            return Ok("Created");
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] CourseCreateDto dto)
        {
            var existingCourse = await _service.GetCourseByCodeAsync(code);
            if (existingCourse == null) return NotFound();

            var hasRegistrations = await _service.HasStudentRegistrationsAsync(code);
            if (hasRegistrations)
            {
                dto.Credits = existingCourse.Credits;
            }
            else {
                existingCourse.Credits = dto.Credits;
            }

            // Cập nhật dữ liệu
            existingCourse.Name = dto.Name;
            existingCourse.Description = dto.Description;
            existingCourse.DepartmentId = dto.DepartmentId;
            existingCourse.PrerequisiteCourseCode = dto.PrerequisiteCourseCode;
            existingCourse.IsActive = dto.IsActive;

            var success = await _service.UpdateCourseAsync(existingCourse);
            if (!success) return BadRequest("Cập nhật thất bại");

            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var existingCourse = await _service.GetCourseByCodeAsync(code);
            if (existingCourse == null) return NotFound();

            var success = await _service.DeleteCourseAsync(code);
            if (!success) return BadRequest("Xóa thất bại");

            return Ok("Xóa thành công");
        }
    }
}
