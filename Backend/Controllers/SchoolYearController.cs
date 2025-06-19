using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using Microsoft.Extensions.Localization;

namespace StudentManagement.Controllers
{
    [Route("api/schoolyears")]
    [ApiController]
    public class SchoolYearController : ControllerBase
    {
        private readonly ISchoolYearService _schoolYearService;
        private readonly ILogger<SchoolYearController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SchoolYearController(
            ISchoolYearService schoolYearService,
            ILogger<SchoolYearController> logger,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _schoolYearService = schoolYearService;
            _logger = logger;
            _localizer = localizer;
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
                        message = _localizer["GetAllSchoolYearsSuccess"].Value,
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
                        message = _localizer["InternalServerError"].Value,
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
                            message = _localizer["SchoolYearNotFound"].Value,
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = schoolYear,
                        message = _localizer["GetSchoolYearSuccess"].Value,
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
                        message = _localizer["InternalServerError"].Value,
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchoolYear(SchoolYear schoolYear)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp =>
                            kvp.Value != null
                                ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                : Array.Empty<string>()
                    );
                return BadRequest(
                    new
                    {
                        data = schoolYear,
                        message = _localizer["InvalidSchoolYearData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            try
            {
                var result = await _schoolYearService.CreateSchoolYearAsync(schoolYear);
                return result == null
                    ? BadRequest(
                        new
                        {
                            data = schoolYear,
                            message = _localizer["SchoolYearNameRequired"].Value,
                            status = "Error",
                        }
                    )
                    : CreatedAtAction(nameof(GetSchoolYear), new { id = result.Id },
                        new
                        {
                            data = result,
                            message = _localizer["CreateSchoolYearSuccess"].Value,
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
                        message = _localizer["InternalServerError"].Value,
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolYear(int id, SchoolYear schoolYear)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp =>
                            kvp.Value != null
                                ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                : Array.Empty<string>()
                    );
                return BadRequest(
                    new
                    {
                        data = schoolYear,
                        message = _localizer["InvalidSchoolYearData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            try
            {
                var updated = await _schoolYearService.UpdateSchoolYearAsync(id, schoolYear);
                return updated
                    ? Ok(
                        new
                        {
                            data = schoolYear,
                            message = _localizer["UpdateSchoolYearSuccess"].Value,
                            status = "Success",
                        }
                    )
                    : BadRequest(
                        new
                        {
                            data = schoolYear,
                            message = _localizer["InvalidSchoolYearId"].Value,
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
                        message = _localizer["InternalServerError"].Value,
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
                            message = _localizer["DeleteSchoolYearSuccess"].Value,
                            status = "Success",
                        }
                    )
                    : BadRequest(
                        new
                        {
                            data = id,
                            message = _localizer["InvalidSchoolYearId"].Value,
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
                        message = _localizer["InternalServerError"].Value,
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }
    }
}