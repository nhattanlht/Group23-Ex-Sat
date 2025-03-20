using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    [Route("api/schoolyears")]
    [ApiController]
    public class SchoolYearController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SchoolYearController> _logger; // Thêm logger

        public SchoolYearController(ApplicationDbContext context, ILogger<SchoolYearController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Lấy danh sách tất cả các năm học
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolYear>>> GetSchoolYears()
        {
            _logger.LogInformation("Fetching all school years.");
            try
            {
                var schoolYears = await _context.SchoolYears.ToListAsync();
                return Ok(schoolYears);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching school years.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Lấy thông tin chi tiết của một năm học theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolYear>> GetSchoolYear(int id)
        {
            _logger.LogInformation("Fetching school year with ID: {ID}", id);
            try
            {
                var schoolYear = await _context.SchoolYears.FindAsync(id);

                if (schoolYear == null)
                {
                    _logger.LogWarning("School year not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy năm học." });
                }

                return Ok(schoolYear);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching school year: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Thêm mới một năm học
        [HttpPost]
        public async Task<ActionResult<SchoolYear>> CreateSchoolYear(SchoolYear schoolYear)
        {
            _logger.LogInformation("Creating new school year: {@SchoolYear}", schoolYear);
            try
            {
                if (string.IsNullOrWhiteSpace(schoolYear.Name))
                {
                    _logger.LogWarning("Invalid school year name.");
                    return BadRequest(new { message = "Tên năm học không được để trống." });
                }

                _context.SchoolYears.Add(schoolYear);
                await _context.SaveChangesAsync();

                _logger.LogInformation("School year created successfully: {ID}", schoolYear.Id);
                return CreatedAtAction(nameof(GetSchoolYear), new { id = schoolYear.Id }, schoolYear);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating school year.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Cập nhật thông tin năm học
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolYear(int id, SchoolYear schoolYear)
        {
            _logger.LogInformation("Updating school year: {ID}, Data: {@SchoolYear}", id, schoolYear);
            try
            {
                if (id != schoolYear.Id)
                {
                    _logger.LogWarning("School year ID mismatch. Provided: {ID}, Actual: {SchoolYearID}", id, schoolYear.Id);
                    return BadRequest(new { message = "ID không hợp lệ." });
                }

                var existingSchoolYear = await _context.SchoolYears.FindAsync(id);
                if (existingSchoolYear == null)
                {
                    _logger.LogWarning("School year not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy năm học." });
                }

                existingSchoolYear.Name = schoolYear.Name;

                try
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("School year updated successfully: {ID}", id);
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error while updating school year: {ID}", id);
                    return StatusCode(500, new { message = "Lỗi cập nhật dữ liệu." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating school year: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Xóa một năm học
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchoolYear(int id)
        {
            _logger.LogInformation("Deleting school year with ID: {ID}", id);
            try
            {
                var schoolYear = await _context.SchoolYears.FindAsync(id);
                if (schoolYear == null)
                {
                    _logger.LogWarning("School year not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy năm học." });
                }

                _context.SchoolYears.Remove(schoolYear);
                await _context.SaveChangesAsync();

                _logger.LogInformation("School year deleted successfully: {ID}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting school year: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
