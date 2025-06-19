using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DepartmentsController(
            IDepartmentService service,
            ILogger<DepartmentsController> logger,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _service = service;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _service.GetAllDepartmentsAsync();
                return Ok(
                    new
                    {
                        data = departments,
                        message = _localizer["GetAllDepartmentsSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching departments.");
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
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                var department = await _service.GetDepartmentByIdAsync(id);
                if (department == null)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = _localizer["DepartmentNotFound"].Value,
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = department,
                        message = _localizer["GetDepartmentSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching department.");
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
        public async Task<IActionResult> CreateDepartment(Department department)
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
                        data = department,
                        message = _localizer["InvalidDepartmentData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            try
            {
                var (exists, message) = await _service.CheckDuplicateAsync(department.Name);
                if (exists)
                    return BadRequest(new { message });

                var created = await _service.CreateAsync(department);
                return CreatedAtAction(
                    nameof(GetDepartment),
                    new { id = created.Id },
                    new
                    {
                        data = created,
                        message = _localizer["CreateDepartmentSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department.");
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
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
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
                        data = department,
                        message = _localizer["InvalidDepartmentData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            try
            {
                if (id != department.Id)
                    return BadRequest(
                        new
                        {
                            data = new { id, department },
                            message = _localizer["DepartmentIdMismatch"].Value,
                            status = "Error",
                        }
                    );

                var updated = await _service.UpdateAsync(id, department);
                if (!updated)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = _localizer["DepartmentNotFound"].Value,
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = department,
                        message = _localizer["UpdateDepartmentSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department.");
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
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = _localizer["DepartmentNotFound"].Value,
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = id,
                        message = _localizer["DeleteDepartmentSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department.");
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
