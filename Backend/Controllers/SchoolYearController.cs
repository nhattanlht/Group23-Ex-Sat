using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/schoolyears")]
    [ApiController]
    public class SchoolYearController : ControllerBase
    {
        private readonly SchoolYearService _schoolYearService;
        private readonly ILogger<SchoolYearController> _logger;

        public SchoolYearController(SchoolYearService schoolYearService, ILogger<SchoolYearController> logger)
        {
            _schoolYearService = schoolYearService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolYears()
        {
            try
            {
                var schoolYears = await _schoolYearService.GetAllSchoolYearsAsync();
                return Ok(schoolYears);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching school years.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolYear(int id)
        {
            try
            {
                var schoolYear = await _schoolYearService.GetSchoolYearByIdAsync(id);
                if (schoolYear == null)
                    return NotFound(new { message = "Không tìm thấy năm học." });

                return Ok(schoolYear);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching school year: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchoolYear(SchoolYear schoolYear)
        {
            try
            {
                var result = await _schoolYearService.CreateSchoolYearAsync(schoolYear);
                return result == null
                    ? BadRequest(new { message = "Tên năm học không được để trống." })
                    : CreatedAtAction(nameof(GetSchoolYear), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating school year.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolYear(int id, SchoolYear schoolYear)
        {
            try
            {
                var updated = await _schoolYearService.UpdateSchoolYearAsync(id, schoolYear);
                return updated
                    ? NoContent()
                    : BadRequest(new { message = "ID không hợp lệ hoặc năm học không tồn tại." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating school year: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchoolYear(int id)
        {
            try
            {
                var deleted = await _schoolYearService.DeleteSchoolYearAsync(id);
                return deleted
                    ? NoContent()
                    : BadRequest(new { message = "Năm học không tồn tại." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting school year: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}