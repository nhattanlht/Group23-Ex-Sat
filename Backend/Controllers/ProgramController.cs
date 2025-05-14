using Microsoft.AspNetCore.Mvc;
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

        public ProgramController(IProgramService programService, ILogger<ProgramController> logger)
        {
            _programService = programService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrograms()
        {
            try
            {
                var programs = await _programService.GetAllProgramsAsync();
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching study programs.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgram(int id)
        {
            try
            {
                var program = await _programService.GetProgramByIdAsync(id);
                if (program == null)
                    return NotFound(new { message = "Không tìm thấy chương trình đào tạo!" });

                return Ok(program);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching study program: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgram(StudyProgram program)
        {
            try
            {
                var result = await _programService.CreateProgramAsync(program);
                return result == null
                    ? BadRequest(new { message = "Chương trình đào tạo đã tồn tại!" })
                    : CreatedAtAction(nameof(GetProgram), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating study program.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, StudyProgram program)
        {
            try
            {
                var updated = await _programService.UpdateProgramAsync(id, program);
                return updated
                    ? NoContent()
                    : BadRequest(new { message = "ID không khớp hoặc chương trình không tồn tại!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating study program: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            try
            {
                var deleted = await _programService.DeleteProgramAsync(id);
                return deleted
                    ? NoContent()
                    : BadRequest(new { message = "Không thể xóa vì có sinh viên thuộc chương trình này hoặc không tìm thấy chương trình!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting study program: {id}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}