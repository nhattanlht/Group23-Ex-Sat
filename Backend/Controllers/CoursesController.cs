using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using Microsoft.Extensions.Localization;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;



        public CourseController(ICourseService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _service.GetAllCoursesAsync();
            return Ok(
                new
                {
                    data = courses,
                    message = _localizer["GetAllCoursesSuccess"].Value,
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
                    message = _localizer["GetActiveCoursesSuccess"].Value,
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
                    message = _localizer["CreateCourseSuccess"].Value,
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
                        message = _localizer["CourseNotFound"].Value,
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
                        message = _localizer["UpdateCourseCreditsError"].Value,
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
                        message = _localizer["CourseCreditsMinError"].Value,
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
                            message = _localizer["PrerequisiteCourseNotFound"].Value,
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
                        message = _localizer["UpdatePrerequisiteCourseError"].Value,
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
                        message = _localizer["UpdateCourseError"].Value,
                        status = "Error",
                    }
                );

            return Ok(
                new
                {
                    data = existingCourse,
                    message = _localizer["UpdateCourseSuccess"].Value,
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
                        message = _localizer["CourseNotFound"].Value,
                        status = "NotFound",
                    }
                );

            var success = await _service.DeleteCourseAsync(code);
            if (!success)
                return BadRequest(
                    new
                    {
                        data = code,
                        message = _localizer["DeleteCourseError"].Value,
                        status = "Error",
                    }
                );

            return Ok(
                new
                {
                    data = code,
                    message = _localizer["DeleteCourseSuccess"].Value,
                    status = "Success",
                }
            );
        }
    }
}
