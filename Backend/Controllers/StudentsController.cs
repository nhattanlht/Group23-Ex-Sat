using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using System;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // Lấy danh sách sinh viên với phân trang
        [HttpGet]
        public async Task<IActionResult> GetStudents(int page = 1, int pageSize = 10)
        {
            try
            {
                var (students, totalStudents, totalPages) = await _studentService.GetStudents(page, pageSize);
                return Ok(new
                {
                    students,
                    totalStudents,
                    totalPages,
                    currentPage = page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Thêm sinh viên mới
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Student student)
        {
            try
            {
                if (student == null)
                    return BadRequest(new { message = "Dữ liệu sinh viên không hợp lệ." });

                var (success, message) = await _studentService.CreateStudent(student);
                if (success)
                    return Ok(new { message });

                return BadRequest(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ", error = ex.Message });
            }
        }


        // Lấy thông tin 1 sinh viên theo MSSV
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            try
            {
                var student = await _studentService.GetStudentById(id);
                if (student == null)
                    return NotFound(new { message = "Student not found." });

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Cập nhật thông tin sinh viên
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Student student)
        {
            try
            {
                if (student == null)
                    return BadRequest(new { message = "Dữ liệu sinh viên không hợp lệ." });

                if (id != student.MSSV)
                    return BadRequest(new { message = "Mã số sinh viên không khớp." });

                var (success, message) = await _studentService.UpdateStudent(student);
                if (success)
                    return Ok(new { message });

                return BadRequest(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ", error = ex.Message });
            }
        }


        // Xóa sinh viên theo MSSV
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _studentService.DeleteStudent(id);
                if (result)
                    return Ok(new { message = "Student deleted successfully." });

                return BadRequest(new { message = "Failed to delete student." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Tìm kiếm sinh viên theo từ khóa
        [HttpGet("search")]
        public async Task<IActionResult> Search(string keyword, int page = 1, int pageSize = 10)
        {
            try
            {
                var (students, totalStudents, totalPages) = await _studentService.SearchStudents(keyword, page, pageSize);
                return Ok(new
                {
                    students,
                    totalStudents,
                    totalPages,
                    currentPage = page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
