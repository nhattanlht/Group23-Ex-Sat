using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ClassController(IClassService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(
                new
                {
                    data = await _service.GetAllAsync(),
                    message = _localizer["GetAllClassesSuccess"].Value,
                    status = "Success",
                }
            );

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetById(string classId)
        {
            var result = await _service.GetByIdAsync(classId);
            if (result == null)
                return NotFound(
                    new
                    {
                        data = classId,
                        message = _localizer["ClassNotFound"].Value,
                        status = "NotFound",
                    }
                );
            return Ok(
                new
                {
                    data = result,
                    message = _localizer["GetClassSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp =>
                            kvp.Value != null
                                ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                : Array.Empty<string>()
                    );
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = _localizer["InvalidClassData"].Value,
                        status = "Error",
                        errors,
                    }
                ); // Nếu dữ liệu không hợp lệ, trả về lỗi 400
            }

            var classEntity = new Class
            {
                ClassId = dto.ClassId,
                CourseCode = dto.CourseCode,
                AcademicYear = dto.AcademicYear,
                Semester = dto.Semester,
                Teacher = dto.Teacher,
                MaxStudents = dto.MaxStudents,
                Schedule = dto.Schedule,
                Classroom = dto.Classroom,
                CancelDeadline = dto.CancelDeadline,
            };
            try
            {
                await _service.AddAsync(classEntity);
                return Ok(
                    new
                    {
                        data = classEntity,
                        message = _localizer["CreateClassSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = ex.Message,
                        status = "Error",
                        error = ex.Message,
                    }
                );
            }
        }

        [HttpPut("{classId}")]
        public async Task<IActionResult> Update(string classId, [FromBody] ClassCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp =>
                            kvp.Value != null
                                ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                : Array.Empty<string>()
                    );
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = _localizer["InvalidClassData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            if (classId != dto.ClassId)
                return BadRequest(
                    new
                    {
                        data = classId,
                        message = _localizer["ClassIdMismatch"].Value,
                        status = "Error",
                    }
                );

            // Tạo đối tượng Class từ DTO
            var classEntity = new Class
            {
                ClassId = dto.ClassId,
                CourseCode = dto.CourseCode,
                AcademicYear = dto.AcademicYear,
                Semester = dto.Semester,
                Teacher = dto.Teacher,
                MaxStudents = dto.MaxStudents,
                Schedule = dto.Schedule,
                Classroom = dto.Classroom,
                CancelDeadline = dto.CancelDeadline,
            };

            // Gọi service để cập nhật
            await _service.UpdateAsync(classEntity);
            return Ok(
                new
                {
                    data = classEntity,
                    message = _localizer["UpdateClassSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpDelete("{classId}")]
        public async Task<IActionResult> Delete(string classId)
        {
            var existingClass = await _service.GetByIdAsync(classId);
            if (existingClass == null)
                return NotFound(
                    new
                    {
                        data = classId,
                        message = _localizer["ClassNotFound"].Value,
                        status = "NotFound",
                    }
                );
            try
            {
                await _service.DeleteAsync(classId);
                return Ok(
                    new
                    {
                        data = classId,
                        message = _localizer["DeleteClassSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        data = classId,
                        message = ex.Message,
                        status = "Error",
                        error = ex.Message,
                    }
                );
            }
        }
    }
}
