using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    [Route("api/student-statuses")]
    [ApiController]
    public class StudentStatusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentStatusController> _logger; // Thêm logger

        public StudentStatusController(ApplicationDbContext context, ILogger<StudentStatusController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 1. Lấy danh sách tất cả các StudentStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentStatus>>> GetStudentStatuses()
        {
            _logger.LogInformation("Fetching all student statuses.");
            try
            {
                var statuses = await _context.StudentStatuses.ToListAsync();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student statuses.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 2. Lấy thông tin một StudentStatus theo Id
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentStatus>> GetStudentStatus(int id)
        {
            _logger.LogInformation("Fetching student status with ID: {ID}", id);
            try
            {
                var studentStatus = await _context.StudentStatuses.FindAsync(id);
                if (studentStatus == null)
                {
                    _logger.LogWarning("Student status not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy tình trạng sinh viên!" });
                }

                return Ok(studentStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student status: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 3. Thêm mới một StudentStatus
        [HttpPost]
        public async Task<ActionResult<StudentStatus>> CreateStudentStatus(StudentStatus studentStatus)
        {
            _logger.LogInformation("Creating student status: {@StudentStatus}", studentStatus);
            try
            {
                if (await _context.StudentStatuses.AnyAsync(s => s.Name == studentStatus.Name))
                {
                    _logger.LogWarning("Student status already exists: {Name}", studentStatus.Name);
                    return BadRequest(new { message = "Tình trạng sinh viên đã tồn tại!" });
                }

                _context.StudentStatuses.Add(studentStatus);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Student status created successfully: {ID}", studentStatus.Id);
                return CreatedAtAction(nameof(GetStudentStatus), new { id = studentStatus.Id }, studentStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating student status.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 4. Cập nhật tên của một StudentStatus
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentStatus(int id, StudentStatus studentStatus)
        {
            _logger.LogInformation("Updating student status: {ID}, Data: {@StudentStatus}", id, studentStatus);
            try
            {
                if (id != studentStatus.Id)
                {
                    _logger.LogWarning("Student status ID mismatch. Provided: {ID}, Actual: {StatusID}", id, studentStatus.Id);
                    return BadRequest(new { message = "ID không khớp!" });
                }

                var existingStatus = await _context.StudentStatuses.FindAsync(id);
                if (existingStatus == null)
                {
                    _logger.LogWarning("Student status not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy tình trạng sinh viên!" });
                }

                existingStatus.Name = studentStatus.Name;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Student status updated successfully: {ID}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating student status: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 5. Xóa một StudentStatus
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentStatus(int id)
        {
            _logger.LogInformation("Deleting student status with ID: {ID}", id);
            try
            {
                var studentStatus = await _context.StudentStatuses
                    .Include(s => s.Students)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (studentStatus == null)
                {
                    _logger.LogWarning("Student status not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy tình trạng sinh viên!" });
                }

                if (studentStatus.Students.Any())
                {
                    _logger.LogWarning("Cannot delete student status {ID} because it's in use.", id);
                    return BadRequest(new { message = "Không thể xóa vì có sinh viên đang sử dụng tình trạng này!" });
                }

                _context.StudentStatuses.Remove(studentStatus);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Student status deleted successfully: {ID}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting student status: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
