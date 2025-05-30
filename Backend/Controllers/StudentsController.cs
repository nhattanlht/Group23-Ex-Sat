using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentManagement.Models;
using StudentManagement.Services;
using System;
using System.Threading.Tasks;
using StudentManagement.DTOs;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(int page = 1, int pageSize = 10)
        {
            _logger.LogInformation("Fetching students (Page: {Page}, PageSize: {PageSize})", page, pageSize);
            try
            {
                var (students, totalStudents, totalPages) = await _studentService.GetStudents(page, pageSize);
                return Ok(new { students, totalStudents, totalPages, currentPage = page, pageSize });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching students.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Student student)
        {
            _logger.LogInformation("Creating student: {@Student}", student);
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning(ModelState.ToString());
                    return BadRequest(new { errors = ModelState });
                }
                if (student == null)
                {
                    _logger.LogWarning("Invalid student data received.");
                    return BadRequest(new { message = "Dữ liệu sinh viên không hợp lệ." });
                }

                var (success, message) = await _studentService.CreateStudent(student);
                if (success)
                {
                    _logger.LogInformation("Student created successfully: {MSSV}", student.MSSV);
                    return Ok(new { message });
                }

                _logger.LogWarning("Failed to create student: {Message}", message);
                return BadRequest(new { message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating student.");
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            _logger.LogInformation("Fetching student with ID: {ID}", id);
            try
            {
                var student = await _studentService.GetStudentById(id);
                if (student == null)
                {
                    _logger.LogWarning("Student not found: {ID}", id);
                    return NotFound(new { message = "Student not found." });
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Student student)
        {
            _logger.LogInformation("Updating student: {ID}, Data: {@Student}", id, student);
            try
            {
                if (student == null)
                {
                    _logger.LogWarning("Invalid student data received.");
                    return BadRequest(new { message = "Dữ liệu sinh viên không hợp lệ." });
                }

                if (id != student.MSSV)
                {
                    _logger.LogWarning("Student ID mismatch. Provided: {ID}, Actual: {MSSV}", id, student.MSSV);
                    return BadRequest(new { message = "Mã số sinh viên không khớp." });
                }

                var (success, message) = await _studentService.UpdateStudent(student);
                if (success)
                {
                    _logger.LogInformation("Student updated successfully: {ID}", id);
                    return Ok(new { message });
                }

                _logger.LogWarning("Failed to update student: {ID}, Message: {Message}", id, message);
                return BadRequest(new { message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating student: {ID}", id);
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting student with ID: {ID}", id);
            try
            {
                var result = await _studentService.DeleteStudent(id);
                if (result)
                {
                    _logger.LogInformation("Student deleted successfully: {ID}", id);
                    return Ok(new { message = "Student deleted successfully." });
                }

                _logger.LogWarning("Failed to delete student: {ID}", id);
                return BadRequest(new { message = "Failed to delete student." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting student: {ID}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] StudentFilterModel filter, int page = 1, int pageSize = 10)
        {
            _logger.LogInformation("Searching students with keyword: {Keyword}, departmentId: {DepartmentId}, Page: {Page}, PageSize: {PageSize}", filter.Keyword, filter.DepartmentId, page, pageSize);
            try
            {
                var (students, totalStudents, totalPages) = await _studentService.SearchStudents(filter, page, pageSize);
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
                _logger.LogError(ex, "Error while searching students with keyword: {Keyword}, departmentId: {DepartmentId}", filter.Keyword, filter.DepartmentId);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}