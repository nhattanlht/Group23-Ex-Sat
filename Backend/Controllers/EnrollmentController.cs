using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _service;

        public EnrollmentController(IEnrollmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _service.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentCreateDto dto)
        {
            var hasPrerequisite = await _service.HasPrerequisiteAsync(dto.StudentId, dto.ClassId);
            if (!hasPrerequisite)
            {
                return BadRequest("Student has not completed the prerequisite course.");
            }
            var isClassFull = await _service.IsClassFullAsync(dto.ClassId);
            if (isClassFull)
            {
                return BadRequest("The class is already full.");
            }
            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                RegisteredAt = DateTime.Now
            };

            await _service.AddAsync(enrollment);
            return Ok("Enrollment created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EnrollmentUpdateDto dto)
        {
            var enrollment = await _service.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound("Không tìm thấy đăng ký");

            if (dto.IsCancelled && !enrollment.IsCancelled)
            {
                var success = await _service.CancelEnrollmentAsync(id, dto.CancelReason ?? "Không có lý do");
                if (!success)
                    return BadRequest("Hủy đăng ký không hợp lệ hoặc đã quá hạn.");

                return Ok("Đã hủy đăng ký thành công.");
            }

            await _service.UpdateAsync(enrollment);
            return Ok("Cập nhật đăng ký thành công.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }

}
