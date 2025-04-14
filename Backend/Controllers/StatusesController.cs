using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/student-statuses")]
    [ApiController]
    public class StudentStatusController : ControllerBase
    {
        private readonly StudentStatusService _studentStatusService;
        private readonly ILogger<StudentStatusController> _logger;

        public StudentStatusController(StudentStatusService studentStatusService, ILogger<StudentStatusController> logger)
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
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student statuses.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentStatus(int id)
        {
            try
            {
                var status = await _studentStatusService.GetStudentStatusByIdAsync(id);
                if (status == null)
                    return NotFound(new { message = "Không tìm thấy tình trạng sinh viên!" });

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching student status: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentStatus(StudentStatus studentStatus)
        {
            try
            {
                var createdStatus = await _studentStatusService.CreateStudentStatusAsync(studentStatus);
                if (createdStatus == null)
                    return BadRequest(new { message = "Tình trạng sinh viên đã tồn tại!" });

                return CreatedAtAction(nameof(GetStudentStatus), new { id = createdStatus.Id }, createdStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating student status.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentStatus(int id, StudentStatus studentStatus)
        {
            try
            {
                var updated = await _studentStatusService.UpdateStudentStatusAsync(id, studentStatus);
                if (!updated)
                    return BadRequest(new { message = "ID không khớp hoặc tình trạng sinh viên không tồn tại!" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating student status: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentStatus(int id)
        {
            try
            {
                var deleted = await _studentStatusService.DeleteStudentStatusAsync(id);
                if (!deleted)
                    return BadRequest(new { message = "Tình trạng sinh viên không tồn tại hoặc có sinh viên đang sử dụng tình trạng này!" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting student status: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}