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
            Ok(
                new
                {
                    data = await _service.GetAllAsync(),
                    message = "Lấy danh sách lớp học thành công.",
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
                        message = "Lớp học không tồn tại.",
                        status = "NotFound",
                    }
                );
            return Ok(
                new
                {
                    data = result,
                    message = "Lấy thông tin lớp học thành công.",
                    status = "Success",
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new
                    {
                        data = ModelState,
                        message = "Dữ liệu không hợp lệ.",
                        status = "Error",
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
                CancelDeadline = dto.CancelDeadline
            };

            await _service.AddAsync(classEntity);
            return Ok(
                new
                {
                    data = classEntity,
                    message = "Lớp học đã được tạo thành công.",
                    status = "Success",
                }
            );
        }

        [HttpPut("{classId}")]
        public async Task<IActionResult> Update(string classId, [FromBody] ClassCreateDto dto)
        {
            if (classId != dto.ClassId)
                return BadRequest(
                    new
                    {
                        data = classId,
                        message = "ID lớp học không khớp.",
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
                CancelDeadline = dto.CancelDeadline
            };

            // Gọi service để cập nhật
            await _service.UpdateAsync(classEntity);
            return Ok(
                new
                {
                    data = classEntity,
                    message = "Lớp học đã được cập nhật thành công.",
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
                        message = "Lớp học không tồn tại.",
                        status = "NotFound",
                    }
                );

            await _service.DeleteAsync(classId);
            return Ok(
                new
                {
                    data = classId,
                    message = "Lớp học đã được xóa thành công.",
                    status = "Success",
                }
            );
        }
    }
}