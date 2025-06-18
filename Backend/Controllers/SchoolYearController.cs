using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/schoolyears")]
    [ApiController]
    public class SchoolYearController : ControllerBase
    {
        private readonly ISchoolYearService _schoolYearService;
        private readonly ILogger<SchoolYearController> _logger;

        public SchoolYearController(
            ISchoolYearService schoolYearService,
            ILogger<SchoolYearController> logger
        )
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
                return Ok(
                    new
                    {
                        data = schoolYears,
                        message = "Lấy danh sách năm học thành công.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching school years.");
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
        public async Task<IActionResult> GetSchoolYear(int id)
        {
            try
            {
                var schoolYear = await _schoolYearService.GetSchoolYearByIdAsync(id);
                if (schoolYear == null)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = "Không tìm thấy năm học.",
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = schoolYear,
                        message = "Lấy thông tin năm học thành công.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching school year: {id}");
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
        public async Task<IActionResult> CreateSchoolYear(SchoolYear schoolYear)
        {
            try
            {
                var result = await _schoolYearService.CreateSchoolYearAsync(schoolYear);
                return result == null
                    ? BadRequest(
                        new
                        {
                            data = schoolYear,
                            message = "Tên năm học không được để trống.",
                            status = "Error",
                        }
                    )
                    : CreatedAtAction(nameof(GetSchoolYear), new { id = result.Id }, 
                        new
                        {
                            data = result,
                            message = "Năm học đã được tạo thành công.",
                            status = "Success",
                        }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating school year.");
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
        public async Task<IActionResult> UpdateSchoolYear(int id, SchoolYear schoolYear)
        {
            try
            {
                var updated = await _schoolYearService.UpdateSchoolYearAsync(id, schoolYear);
                return updated
                    ? Ok(
                        new
                        {
                            data = schoolYear,
                            message = "Cập nhật năm học thành công.",
                            status = "Success",
                        }
                    )
                    : BadRequest(
                        new
                        {
                            data = schoolYear,
                            message = "ID không hợp lệ hoặc năm học không tồn tại.",
                            status = "Error",
                        }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating school year: {id}");
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
        public async Task<IActionResult> DeleteSchoolYear(int id)
        {
            try
            {
                var deleted = await _schoolYearService.DeleteSchoolYearAsync(id);
                return deleted
                    ? Ok(
                        new
                        {
                            data = id,
                            message = "Xóa năm học thành công.",
                            status = "Success",
                        }
                    )
                    : BadRequest(
                        new
                        {
                            data = id,
                            message = "Năm học không tồn tại.",
                            status = "Error",
                        }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting school year: {id}");
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