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
        public async Task<IActionResult> Create([FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    Console.WriteLine("Student data is null.");
                    return BadRequest(new { message = "Student data is required." });
                }

                // Log chi tiết dữ liệu nhận được
                Console.WriteLine("Received Student Data: " + Newtonsoft.Json.JsonConvert.SerializeObject(student));

                var result = await _studentService.CreateStudent(student);
                if (result)
                {
                    Console.WriteLine("Student created successfully.");
                    return Ok(new { message = "Student created successfully." });
                }

                Console.WriteLine("Failed to create student.");
                return BadRequest(new { message = "Failed to create student." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
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
                    return BadRequest(new { message = "Student data is required." });

                if (id != student.MSSV)
                    return BadRequest(new { message = "Student ID mismatch." });

                var result = await _studentService.UpdateStudent(student);
                if (result)
                    return Ok(new { message = "Student updated successfully." });

                return BadRequest(new { message = "Failed to update student." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
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
                var students = await _studentService.SearchStudents(keyword, page, pageSize);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
