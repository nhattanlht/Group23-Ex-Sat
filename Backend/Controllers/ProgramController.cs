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

        public ProgramController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách tất cả chương trình đào tạo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudyProgram>>> GetPrograms()
        {
            return await _context.StudyPrograms.ToListAsync();
        }

        // 2. Lấy thông tin một chương trình đào tạo theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<StudyProgram>> GetProgram(int id)
        {
            var program = await _context.StudyPrograms.FindAsync(id);

            if (program == null)
            {
                return NotFound(new { message = "Không tìm thấy chương trình đào tạo!" });
            }

            return program;
        }

        // 3. Thêm mới một chương trình đào tạo
        [HttpPost]
        public async Task<ActionResult<StudyProgram>> CreateProgram(StudyProgram program)
        {
            if (await _context.StudyPrograms.AnyAsync(p => p.Name == program.Name))
            {
                return BadRequest(new { message = "Chương trình đào tạo đã tồn tại!" });
            }

            _context.StudyPrograms.Add(program);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
        }

        // 4. Cập nhật thông tin chương trình đào tạo
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, StudyProgram program)
        {
            if (id != program.Id)
            {
                return BadRequest(new { message = "ID không khớp!" });
            }

            var existingProgram = await _context.StudyPrograms.FindAsync(id);
            if (existingProgram == null)
            {
                return NotFound(new { message = "Không tìm thấy chương trình đào tạo!" });
            }

            existingProgram.Name = program.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. Xóa một chương trình đào tạo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var program = await _context.StudyPrograms
                .Include(p => p.Students)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (program == null)
            {
                return NotFound(new { message = "Không tìm thấy chương trình đào tạo!" });
            }

            if (program.Students.Any())
            {
                return BadRequest(new { message = "Không thể xóa vì có sinh viên thuộc chương trình này!" });
            }

            _context.StudyPrograms.Remove(program);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
