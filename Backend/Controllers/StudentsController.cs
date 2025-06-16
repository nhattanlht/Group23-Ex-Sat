using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public StudentsController(
            IStudentService studentService,
            ILogger<StudentsController> logger,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _studentService = studentService;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(int page = 1, int pageSize = 10)
        {
            _logger.LogInformation(
                "Fetching students (Page: {Page}, PageSize: {PageSize})",
                page,
                pageSize
            );
            try
            {
                var (students, totalStudents, totalPages) = await _studentService.GetStudents(
                    page,
                    pageSize
                );
                return Ok(
                    new
                    {
                        data = new
                        {
                            students,
                            totalStudents,
                            totalPages,
                            currentPage = page,
                            pageSize,
                        },
                        message = _localizer["GetStudentsSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching students.");
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
        public async Task<IActionResult> Create([FromBody] Student student)
        {
            _logger.LogInformation("Creating student: {@Student}", student);
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning(ModelState.ToString());
                    return BadRequest(
                        new
                        {
                            data = ModelState,
                            message = _localizer["InvalidModelState"].Value,
                            status = "Error",
                        }
                    );
                }
                if (student == null)
                {
                    _logger.LogWarning("Invalid student data received.");
                    return BadRequest(
                        new
                        {
                            data = student,
                            message = _localizer["InvalidStudentData"].Value,
                            status = "Error",
                        }
                    );
                }

                var (success, message) = await _studentService.CreateStudent(student);
                if (success)
                {
                    _logger.LogInformation(
                        "Student created successfully: {StudentId}",
                        student.StudentId
                    );
                    return Ok(
                        new
                        {
                            data = student,
                            message,
                            status = "Success",
                        }
                    );
                }

                _logger.LogWarning("Failed to create student: {Message}", message);
                return BadRequest(
                    new
                    {
                        data = student,
                        message,
                        status = "Error",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating student.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = _localizer["InternalServerError"].Value,
                        status = "Error",
                        errors = ex.Message,
                    }
                );
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
                    return NotFound(
                        new
                        {
                            data = id,
                            message = _localizer["StudentNotFound"].Value,
                            status = "NotFound",
                        }
                    );
                }

                return Ok(
                    new
                    {
                        data = student,
                        message = _localizer["GetStudentSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student: {ID}", id);
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
        public async Task<IActionResult> Edit(string id, [FromBody] Student student)
        {
            _logger.LogInformation("Updating student: {ID}, Data: {@Student}", id, student);
            try
            {
                if (student == null)
                {
                    _logger.LogWarning("Invalid student data received.");
                    return BadRequest(
                        new
                        {
                            data = student,
                            message = _localizer["InvalidStudentData"].Value,
                            status = "Error",
                        }
                    );
                }

                if (id != student.StudentId)
                {
                    _logger.LogWarning(
                        "Student ID mismatch. Provided: {ID}, Actual: {StudentId}",
                        id,
                        student.StudentId
                    );
                    return BadRequest(
                        new
                        {
                            data = student,
                            message = _localizer["StudentIdMismatch"].Value,
                            status = "Error",
                        }
                    );
                }

                var (success, message) = await _studentService.UpdateStudent(student);
                if (success)
                {
                    _logger.LogInformation("Student updated successfully: {ID}", id);
                    return Ok(
                        new
                        {
                            data = student,
                            message,
                            status = "Success",
                        }
                    );
                }

                _logger.LogWarning(
                    "Failed to update student: {ID}, Message: {Message}",
                    id,
                    message
                );
                return BadRequest(
                    new
                    {
                        data = student,
                        message,
                        status = "Error",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating student: {ID}", id);
                return StatusCode(
                    500,
                    new
                    {
                        message = _localizer["InternalServerError"].Value,
                        data = ex.Message,
                        status = "Error",
                    }
                );
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
                    return Ok(
                        new
                        {
                            data = id,
                            message = _localizer["DeleteStudentSuccess"].Value,
                            status = "Success",
                        }
                    );
                }

                _logger.LogWarning("Failed to delete student: {ID}", id);
                return BadRequest(
                    new
                    {
                        data = id,
                        message = _localizer["DeleteStudentError"].Value,
                        status = "Error",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting student: {ID}", id);
                return StatusCode(
                    500,
                    new
                    {
                        message = _localizer["InternalServerError"].Value,
                        data = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] StudentFilterModel filter,
            int page = 1,
            int pageSize = 10
        )
        {
            _logger.LogInformation(
                "Searching students with keyword: {Keyword}, departmentId: {DepartmentId}, Page: {Page}, PageSize: {PageSize}",
                filter.Keyword,
                filter.DepartmentId,
                page,
                pageSize
            );
            try
            {
                var (students, totalStudents, totalPages) = await _studentService.SearchStudents(
                    filter,
                    page,
                    pageSize
                );
                return Ok(
                    new
                    {
                        data = new
                        {
                            students,
                            totalStudents,
                            totalPages,
                            currentPage = page,
                            pageSize,
                        },
                        message = _localizer["SearchStudentsSuccess"].Value,
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while searching students with keyword: {Keyword}, departmentId: {DepartmentId}",
                    filter.Keyword,
                    filter.DepartmentId
                );
                return StatusCode(
                    500,
                    new
                    {
                        message = _localizer["InternalServerError"].Value,
                        data = ex.Message,
                        status = "Error",
                    }
                );
            }
        }
    }
}
