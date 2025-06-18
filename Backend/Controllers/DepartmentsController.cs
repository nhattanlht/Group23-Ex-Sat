using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(
            IDepartmentService service,
            ILogger<DepartmentsController> logger
        )
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _service.GetAllDepartmentsAsync();
                return Ok(
                    new
                    {
                        data = departments,
                        message = "Lấy danh sách khoa thành công.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching departments.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Internal server error",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                var department = await _service.GetDepartmentByIdAsync(id);
                if (department == null)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = "Không tìm thấy khoa!",
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = department,
                        message = "Lấy thông tin khoa thành công.",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching department.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            try
            {
                var (exists, message) = await _service.CheckDuplicateAsync(department.Name);
                if (exists)
                    return BadRequest(new { message });

                var created = await _service.CreateAsync(department);
                return CreatedAtAction(
                    nameof(GetDepartment),
                    new { id = created.Id },
                    new
                    {
                        data = created,
                        message = "Tạo mới thành công",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            try
            {
                if (id != department.Id)
                    return BadRequest(new { message = "ID không khớp!" });

                var updated = await _service.UpdateAsync(id, department);
                if (!updated)
                    return NotFound(new { message = "Không tìm thấy khoa!" });

                return Ok(
                    new
                    {
                        data = department,
                        message = "Cập nhật thành công",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound(
                        new
                        {
                            data = id,
                            message = "Không tìm thấy khoa để xóa!",
                            status = "NotFound",
                        }
                    );

                return Ok(
                    new
                    {
                        data = id,
                        message = "Xóa khoa thành công",
                        status = "Success",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department.");
                return StatusCode(
                    500,
                    new
                    {
                        data = new { },
                        message = "Lỗi máy chủ nội bộ",
                        errors = ex.Message,
                        status = "Error",
                    }
                );
            }
        }
    }
}
