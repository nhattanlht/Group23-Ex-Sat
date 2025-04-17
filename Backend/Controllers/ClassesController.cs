using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly ClassService _service;

        public ClassController(ClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{classCode}")]
        public async Task<IActionResult> GetById(string classCode)
        {
            var result = await _service.GetByIdAsync(classCode);
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
                Classroom = dto.Classroom
            };

            await _service.AddAsync(classEntity);
            return Ok("Class created successfully");
        }

        [HttpPut("{classCode}")]
        public async Task<IActionResult> Update(string classCode, [FromBody] ClassCreateDto dto)
        {
            if (classCode != dto.ClassId) return BadRequest("Class ID mismatch");

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
                Classroom = dto.Classroom
            };

            // Gọi service để cập nhật
            await _service.UpdateAsync(classEntity);
            return Ok("Class updated successfully");
        }
    }
}