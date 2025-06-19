using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/programs")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;
        private readonly ILogger<ProgramController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProgramController(
            IProgramService programService,
            ILogger<ProgramController> logger,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _programService = programService;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrograms()
        {
            try
            {
                var programs = await _programService.GetAllProgramsAsync();
                return Ok(
                    new
                    {
                        data = programs,
                        message = _localizer["GetAllProgramsSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching study programs.");
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
        public async Task<IActionResult> GetProgram(int id)
        {
            try
            {
                var program = await _programService.GetProgramByIdAsync(id);
                if (program == null)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = _localizer["ProgramNotFound"].Value,
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = program,
                        message = _localizer["GetProgramSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching study program: {id}");
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
        public async Task<IActionResult> CreateProgram(StudyProgram program)
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
                        data = program,
                        message = _localizer["InvalidProgramData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            try
            {
                var result = await _programService.CreateProgramAsync(program);
                return result == null
                    ? BadRequest(
                        new
                        {
                            data = program,
                            message = _localizer["CreateProgramExists"].Value,
                            status = "Error",
                        }
                    )
                    : CreatedAtAction(
                        nameof(GetProgram),
                        new { id = result.Id },
                        new
                        {
                            data = result,
                            message = _localizer["CreateProgramSuccess"].Value,
                            status = "Success",
                        }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating study program.");
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
        public async Task<IActionResult> UpdateProgram(int id, StudyProgram program)
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
                        data = program,
                        message = _localizer["InvalidProgramData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            try
            {
                var updated = await _programService.UpdateProgramAsync(id, program);
                return updated
                    ? Ok(
                        new
                        {
                            data = program,
                            message = _localizer["UpdateProgramSuccess"].Value,
                            status = "Success",
                        }
                    )
                    : BadRequest(
                        new
                        {
                            data = id,
                            message = _localizer["UpdateProgramIdMismatch"].Value,
                            status = "Error",
                        }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating study program: {id}");
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
        public async Task<IActionResult> DeleteProgram(int id)
        {
            try
            {
                var deleted = await _programService.DeleteProgramAsync(id);
                return deleted
                    ? Ok(
                        new
                        {
                            data = id,
                            message = _localizer["DeleteProgramSuccess"].Value,
                            status = "Success",
                        }
                    )
                    : BadRequest(
                        new
                        {
                            data = id,
                            message = _localizer["DeleteProgramError"].Value,
                            status = "Error",
                        }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting study program: {id}");
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
