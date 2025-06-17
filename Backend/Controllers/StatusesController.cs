using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentStatusController : ControllerBase
    {
        private readonly IStudentStatusService _studentStatusService;
        private readonly ILogger<StudentStatusController> _logger;

        public StudentStatusController(
            IStudentStatusService studentStatusService,
            ILogger<StudentStatusController> logger
        )
        {
            _studentStatusService = studentStatusService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentStatuses()
        {
            try
            {
                var statuses = await _studentStatusService.GetAllStudentStatusesAsync();
                return Ok(
                    new
                    {
                        data = statuses,
                        message = "Lấy danh sách tình trạng sinh viên thành công.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student statuses.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentStatus(int id)
        {
            try
            {
                var status = await _studentStatusService.GetStudentStatusByIdAsync(id);
                if (status == null)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = "Không tìm thấy tình trạng sinh viên!",
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = status,
                        message = "Tình trạng sinh viên đã được tìm thấy.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching student status: {id}");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentStatus(StudentStatus studentStatus)
        {
            try
            {
                var createdStatus = await _studentStatusService.CreateStudentStatusAsync(
                    studentStatus
                );
                if (createdStatus == null)
                    return BadRequest(
                        new
                        {
                            data = studentStatus,
                            message = "Tình trạng sinh viên đã tồn tại!",
                            status = "Error",
                        }
                    );

                return CreatedAtAction(
                    nameof(GetStudentStatus),
                    new { id = createdStatus.Id },
                    createdStatus
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating student status.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentStatus(int id, StudentStatus studentStatus)
        {
            try
            {
                var updated = await _studentStatusService.UpdateStudentStatusAsync(
                    id,
                    studentStatus
                );
                if (!updated)
                    return BadRequest(
                        new
                        {
                            data = studentStatus,
                            message = "ID không khớp hoặc tình trạng sinh viên không tồn tại!",
                            status = "Error",
                        }
                    );

                return Ok(
                    new
                    {
                        data = studentStatus,
                        message = "Tình trạng sinh viên đã được cập nhật thành công.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating student status: {id}");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentStatus(int id)
        {
            try
            {
                var deleted = await _studentStatusService.DeleteStudentStatusAsync(id);
                if (!deleted)
                    return BadRequest(
                        new
                        {
                            data = id,
                            message = "Tình trạng sinh viên không tồn tại hoặc có sinh viên đang sử dụng tình trạng này!",
                            status = "Error",
                        }
                    );

                return Ok(
                    new
                    {
                        data = id,
                        message = "Tình trạng sinh viên đã được xóa thành công.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting student status: {id}");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }
    }
}