using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _service.GetAllCoursesAsync();
            return Ok(
                new
                {
                    data = courses,
                    message = "Lấy danh sách khóa học thành công.",
                    status = "Success",
                }
            );
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCourses()
        {
            var courses = await _service.GetActiveCoursesAsync();
            return Ok(
                new
                {
                    data = courses,
                    message = "Lấy danh sách khóa học đang hoạt động thành công.",
                    status = "Success",
                }
            );
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var course = await _service.GetCourseByCodeAsync(code);
            if (course == null)
                return NotFound(
                    new
                    {
                        data = code,
                        message = "Khóa học không tồn tại.",
                        status = "NotFound",
                    }
                );
            return Ok(
                new
                {
                    data = course,
                    message = "Lấy thông tin khóa học thành công.",
                    status = "Success",
                }
            );
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
            return Ok(
                new
                {
                    data = course,
                    message = "Khóa học đã được tạo thành công.",
                    status = "Success",
                }
            );
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] CourseCreateDto dto)
        {
            var existingCourse = await _service.GetCourseByCodeAsync(code);
            if (existingCourse == null)
                return NotFound(
                    new
                    {
                        data = code,
                        message = "Khóa học không tồn tại.",
                        status = "NotFound",
                    }
                );

            var hasRegistrations = await _service.HasStudentRegistrationsAsync(code);
            if (hasRegistrations && dto.Credits != existingCourse.Credits)
            {
                return BadRequest(
                    new
                    {
                        data = code,
                        message = "Không thể cập nhật số tín chỉ của khóa học đã có sinh viên đăng ký",
                        status = "Error",
                    }
                );
            }
            else if (dto.Credits < 2)
            {
                return BadRequest(
                    new
                    {
                        data = dto.Credits,
                        message = "Số tín chỉ phải lớn hơn hoặc bằng 2",
                        status = "Error",
                    }
                );
            }
            else if (
                !string.IsNullOrEmpty(dto.PrerequisiteCourseCode)
                && dto.PrerequisiteCourseCode != existingCourse.PrerequisiteCourseCode
            )
            {
                var prereqExists = await _service.GetCourseByCodeAsync(dto.PrerequisiteCourseCode);
                if (prereqExists == null)
                    return BadRequest(
                        new
                        {
                            data = dto.PrerequisiteCourseCode,
                            message = "Khóa học tiên quyết không tồn tại",
                            status = "Error",
                        }
                    );
            }
            else if (
                hasRegistrations
                && dto.PrerequisiteCourseCode != existingCourse.PrerequisiteCourseCode
            )
            {
                return BadRequest(
                    new
                    {
                        data = code,
                        message = "Không thể cập nhật khóa học tiên quyết của khóa học đã có sinh viên đăng ký",
                        status = "Error",
                    }
                );
            }
            else
            {
                existingCourse.Credits = dto.Credits;
            }

            // Cập nhật dữ liệu
            existingCourse.Name = dto.Name;
            existingCourse.Description = dto.Description;
            existingCourse.DepartmentId = dto.DepartmentId;
            existingCourse.PrerequisiteCourseCode = dto.PrerequisiteCourseCode;
            existingCourse.IsActive = dto.IsActive;

            var success = await _service.UpdateCourseAsync(existingCourse);
            if (!success)
                return BadRequest(
                    new
                    {
                        data = code,
                        message = "Cập nhật khóa học thất bại",
                        status = "Error",
                    }
                );

            return Ok(
                new
                {
                    data = existingCourse,
                    message = "Cập nhật khóa học thành công",
                    status = "Success",
                }
            );
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var existingCourse = await _service.GetCourseByCodeAsync(code);
            if (existingCourse == null)
                return NotFound(
                    new
                    {
                        data = code,
                        message = "Khóa học không tồn tại.",
                        status = "NotFound",
                    }
                );

            var success = await _service.DeleteCourseAsync(code);
            if (!success)
                return BadRequest(
                    new
                    {
                        data = code,
                        message = "Xóa khóa học thất bại. Vui lòng kiểm tra lại.",
                        status = "Error",
                    }
                );

            return Ok(
                new
                {
                    data = code,
                    message = "Khóa học đã được xóa thành công.",
                    status = "Success",
                }
            );
        }
    }
}
