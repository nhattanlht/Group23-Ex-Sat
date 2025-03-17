using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Collections.Generic;

namespace StudentManagement.Controllers
{
    [Route("api/student-statuses")]
    [ApiController]
    public class StudentStatusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách tất cả các StudentStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentStatus>>> GetStudentStatuses()
        {
            return await _context.StudentStatuses.ToListAsync();
        }

        // 2. Lấy thông tin một StudentStatus theo Id
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentStatus>> GetStudentStatus(int id)
        {
            var studentStatus = await _context.StudentStatuses.FindAsync(id);

            if (studentStatus == null)
            {
                return NotFound(new { message = "Không tìm thấy tình trạng sinh viên!" });
            }

            return studentStatus;
        }

        // 3. Thêm mới một StudentStatus
        [HttpPost]
        public async Task<ActionResult<StudentStatus>> CreateStudentStatus(StudentStatus studentStatus)
        {
            if (await _context.StudentStatuses.AnyAsync(s => s.Name == studentStatus.Name))
            {
                return BadRequest(new { message = "Tình trạng sinh viên đã tồn tại!" });
            }

            _context.StudentStatuses.Add(studentStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentStatus), new { id = studentStatus.Id }, studentStatus);
        }

        // 4. Cập nhật tên của một StudentStatus
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentStatus(int id, StudentStatus studentStatus)
        {
            if (id != studentStatus.Id)
            {
                return BadRequest(new { message = "ID không khớp!" });
            }

            var existingStatus = await _context.StudentStatuses.FindAsync(id);
            if (existingStatus == null)
            {
                return NotFound(new { message = "Không tìm thấy tình trạng sinh viên!" });
            }

            existingStatus.Name = studentStatus.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. Xóa một StudentStatus
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentStatus(int id)
        {
            var studentStatus = await _context.StudentStatuses
                .Include(s => s.Students)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (studentStatus == null)
            {
                return NotFound(new { message = "Không tìm thấy tình trạng sinh viên!" });
            }

            if (studentStatus.Students.Any())
            {
                return BadRequest(new { message = "Không thể xóa vì có sinh viên đang sử dụng tình trạng này!" });
            }

            _context.StudentStatuses.Remove(studentStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
