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
            return Ok(
                new
                {
                    data = enrollments,
                    message = "Lấy danh sách đăng ký thành công.",
                    status = "Success",
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(
                    new
                    {
                        data = id,
                        message = "Đăng ký không tồn tại.",
                        status = "NotFound",
                    }
                );
            return Ok(
                new
                {
                    data = result,
                    message = "Lấy thông tin đăng ký thành công.",
                    status = "Success",
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentCreateDto dto)
        {
            var hasPrerequisite = await _service.HasPrerequisiteAsync(dto.StudentId, dto.ClassId);
            if (!hasPrerequisite)
            {
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = "Học viên chưa hoàn thành các khóa học tiên quyết.",
                        status = "Error",
                    }
                );
            }
            var isClassFull = await _service.IsClassFullAsync(dto.ClassId);
            if (isClassFull)
            {
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = "Lớp học đã đầy.",
                        status = "Error",
                    }
                );
            }
            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                RegisteredAt = DateTime.Now,
            };

            await _service.AddAsync(enrollment);
            return Ok(
                new
                {
                    data = enrollment,
                    message = "Đăng ký thành công.",
                    status = "Success",
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EnrollmentUpdateDto dto)
        {
            var enrollment = await _service.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound(
                    new
                    {
                        data = id,
                        message = "Đăng ký không tồn tại.",
                        status = "NotFound",
                    }
                );

            if (dto.IsCancelled && !enrollment.IsCancelled)
            {
                var success = await _service.CancelEnrollmentAsync(
                    id,
                    dto.CancelReason ?? "Không có lý do"
                );
                if (!success)
                    return BadRequest(
                        new
                        {
                            data = id,
                            message = "Không thể hủy đăng ký do đã quá hạn.",
                            status = "Error",
                        }
                    );

                return Ok(
                    new
                    {
                        data = id,
                        message = "Hủy đăng ký thành công.",
                        status = "Success",
                    }
                );
            }

            await _service.UpdateAsync(enrollment);
            return Ok(
                new
                {
                    data = enrollment,
                    message = "Cập nhật đăng ký thành công.",
                    status = "Success",
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(
                new
                {
                    data = id,
                    message = "Đăng ký đã được xóa thành công.",
                    status = "Success",
                }
            );
        }
    }
}
