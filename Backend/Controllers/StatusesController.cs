using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using Microsoft.Extensions.Localization;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentStatusController : ControllerBase
    {
        private readonly IStudentStatusService _studentStatusService;
        private readonly ILogger<StudentStatusController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public StudentStatusController(
            IStudentStatusService studentStatusService,
            ILogger<StudentStatusController> logger,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _studentStatusService = studentStatusService;
            _logger = logger;
            _localizer = localizer;
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
                        message = _localizer["GetAllStudentStatusesSuccess"].Value,
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
                        message = _localizer["InternalServerError"].Value,
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
                            message = _localizer["StudentStatusNotFound"].Value,
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = status,
                        message = _localizer["GetStudentStatusSuccess"].Value,
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
                        message = _localizer["InternalServerError"].Value,
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentStatus(StudentStatus studentStatus)
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
                        data = studentStatus,
                        message = _localizer["InvalidStudentStatusData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
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
                            message = _localizer["CreateStudentStatusExists"].Value,
                            status = "Error",
                        }
                    );

                return CreatedAtAction(
                    nameof(GetStudentStatus),
                    new { id = createdStatus.Id },
                    new
                    {
                        data = createdStatus,
                        message = _localizer["CreateStudentStatusSuccess"].Value,
                        status = "Success",
                    }
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
                        message = _localizer["InternalServerError"].Value,
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentStatus(int id, StudentStatus studentStatus)
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
                        data = studentStatus,
                        message = _localizer["InvalidStudentStatusData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
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
                            message = _localizer["StudentStatusNotFound"].Value,
                            status = "Error",
                        }
                    );

                return Ok(
                    new
                    {
                        data = studentStatus,
                        message = _localizer["UpdateStudentStatusSuccess"].Value,
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
                        message = _localizer["InternalServerError"].Value,
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
                            message = _localizer["DeleteStudentStatusError"].Value,
                            status = "Error",
                        }
                    );

                return Ok(
                    new
                    {
                        data = id,
                        message = _localizer["DeleteStudentStatusSuccess"].Value,
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
                        message = _localizer["InternalServerError"].Value,
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }
    }
}