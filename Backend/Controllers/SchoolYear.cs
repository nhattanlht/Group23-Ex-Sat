using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Route("api/schoolyears")]
    [ApiController]
    public class SchoolYearController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SchoolYearController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả các năm học
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolYear>>> GetSchoolYears()
        {
            return await _context.SchoolYears.ToListAsync();
        }

        // Lấy thông tin chi tiết của một năm học theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolYear>> GetSchoolYear(int id)
        {
            var schoolYear = await _context.SchoolYears.FindAsync(id);

            if (schoolYear == null)
            {
                return NotFound(new { message = "Không tìm thấy năm học." });
            }

            return schoolYear;
        }

        // Thêm mới một năm học
        [HttpPost]
        public async Task<ActionResult<SchoolYear>> CreateSchoolYear(SchoolYear schoolYear)
        {
            if (string.IsNullOrWhiteSpace(schoolYear.Name))
            {
                return BadRequest(new { message = "Tên năm học không được để trống." });
            }

            _context.SchoolYears.Add(schoolYear);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSchoolYear), new { id = schoolYear.Id }, schoolYear);
        }

        // Cập nhật thông tin năm học
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolYear(int id, SchoolYear schoolYear)
        {
            if (id != schoolYear.Id)
            {
                return BadRequest(new { message = "ID không hợp lệ." });
            }

            var existingSchoolYear = await _context.SchoolYears.FindAsync(id);
            if (existingSchoolYear == null)
            {
                return NotFound(new { message = "Không tìm thấy năm học." });
            }

            existingSchoolYear.Name = schoolYear.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Lỗi cập nhật dữ liệu." });
            }

            return NoContent();
        }

        // Xóa một năm học
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchoolYear(int id)
        {
            var schoolYear = await _context.SchoolYears.FindAsync(id);
            if (schoolYear == null)
            {
                return NotFound(new { message = "Không tìm thấy năm học." });
            }

            _context.SchoolYears.Remove(schoolYear);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
