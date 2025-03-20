using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    [Route("api/programs")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProgramController> _logger; // Thêm logger

        public ProgramController(ApplicationDbContext context, ILogger<ProgramController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 1. Lấy danh sách tất cả chương trình đào tạo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudyProgram>>> GetPrograms()
        {
            _logger.LogInformation("Fetching all study programs.");
            try
            {
                var programs = await _context.StudyPrograms.ToListAsync();
                return Ok(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching study programs.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 2. Lấy thông tin một chương trình đào tạo theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<StudyProgram>> GetProgram(int id)
        {
            _logger.LogInformation("Fetching study program with ID: {ID}", id);
            try
            {
                var program = await _context.StudyPrograms.FindAsync(id);

                if (program == null)
                {
                    _logger.LogWarning("Study program not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy chương trình đào tạo!" });
                }

                return Ok(program);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching study program: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 3. Thêm mới một chương trình đào tạo
        [HttpPost]
        public async Task<ActionResult<StudyProgram>> CreateProgram(StudyProgram program)
        {
            _logger.LogInformation("Creating new study program: {@Program}", program);
            try
            {
                if (await _context.StudyPrograms.AnyAsync(p => p.Name == program.Name))
                {
                    _logger.LogWarning("Study program already exists: {Name}", program.Name);
                    return BadRequest(new { message = "Chương trình đào tạo đã tồn tại!" });
                }

                _context.StudyPrograms.Add(program);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Study program created successfully: {ID}", program.Id);
                return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating study program.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 4. Cập nhật thông tin chương trình đào tạo
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, StudyProgram program)
        {
            _logger.LogInformation("Updating study program: {ID}, Data: {@Program}", id, program);
            try
            {
                if (id != program.Id)
                {
                    _logger.LogWarning("Study program ID mismatch. Provided: {ID}, Actual: {ProgramID}", id, program.Id);
                    return BadRequest(new { message = "ID không khớp!" });
                }

                var existingProgram = await _context.StudyPrograms.FindAsync(id);
                if (existingProgram == null)
                {
                    _logger.LogWarning("Study program not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy chương trình đào tạo!" });
                }

                existingProgram.Name = program.Name;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Study program updated successfully: {ID}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating study program: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // 5. Xóa một chương trình đào tạo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            _logger.LogInformation("Deleting study program with ID: {ID}", id);
            try
            {
                var program = await _context.StudyPrograms
                    .Include(p => p.Students)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (program == null)
                {
                    _logger.LogWarning("Study program not found: {ID}", id);
                    return NotFound(new { message = "Không tìm thấy chương trình đào tạo!" });
                }

                if (program.Students.Any())
                {
                    _logger.LogWarning("Cannot delete study program {ID} because it has students enrolled.", id);
                    return BadRequest(new { message = "Không thể xóa vì có sinh viên thuộc chương trình này!" });
                }

                _context.StudyPrograms.Remove(program);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Study program deleted successfully: {ID}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting study program: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
