using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _service;

        public ClassController(IClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetById(string classId)
        {
            var result = await _service.GetByIdAsync(classId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Nếu dữ liệu không hợp lệ, trả về lỗi 400
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
                CancelDeadline = dto.CancelDeadline
            };

            await _service.AddAsync(classEntity);
            return Ok("Class created successfully");
        }

        [HttpPut("{classId}")]
        public async Task<IActionResult> Update(string classId, [FromBody] ClassCreateDto dto)
        {
            if (classId != dto.ClassId) return BadRequest("Class ID mismatch");

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
                CancelDeadline = dto.CancelDeadline
            };

            // Gọi service để cập nhật
            await _service.UpdateAsync(classEntity);
            return Ok("Class updated successfully");
        }
        [HttpDelete("{classId}")]
        public async Task<IActionResult> Delete(string classId)
        {
            var existingClass = await _service.GetByIdAsync(classId);
            if (existingClass == null) return NotFound();

            await _service.DeleteAsync(classId);
            return Ok("Class deleted successfully");
        }
    }
}