using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(ApplicationDbContext context, ILogger<DepartmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // 1. Lấy danh sách tất cả khoa
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        _logger.LogInformation("Fetching all departments.");
        try
        {
            var departments = await _context.Departments.ToListAsync();
            return Ok(departments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching departments.");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    // 2. Lấy thông tin một khoa theo ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartment(int id)
    {
        _logger.LogInformation("Fetching department with ID: {ID}", id);
        try
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                _logger.LogWarning("Department not found: {ID}", id);
                return NotFound(new { message = "Không tìm thấy khoa!" });
            }

            return Ok(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching department: {ID}", id);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    // 3. Thêm mới một khoa
    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartment(Department department)
    {
        _logger.LogInformation("Creating new department: {@Department}", department);
        try
        {
            if (await _context.Departments.AnyAsync(d => d.Name == department.Name))
            {
                _logger.LogWarning("Department already exists: {Name}", department.Name);
                return BadRequest(new { message = "Khoa đã tồn tại!" });
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Department created successfully: {ID}", department.Id);
            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating department.");
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    // 4. Cập nhật thông tin khoa
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, Department department)
    {
        _logger.LogInformation("Updating department: {ID}, Data: {@Department}", id, department);
        try
        {
            if (id != department.Id)
            {
                _logger.LogWarning("Department ID mismatch. Provided: {ID}, Actual: {DepartmentID}", id, department.Id);
                return BadRequest(new { message = "ID không khớp!" });
            }

            var existingDepartment = await _context.Departments.FindAsync(id);
            if (existingDepartment == null)
            {
                _logger.LogWarning("Department not found: {ID}", id);
                return NotFound(new { message = "Không tìm thấy khoa!" });
            }

            existingDepartment.Name = department.Name;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Department updated successfully: {ID}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating department: {ID}", id);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    // 5. Xóa một khoa
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        _logger.LogInformation("Deleting department with ID: {ID}", id);
        try
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                _logger.LogWarning("Department not found: {ID}", id);
                return NotFound(new { message = "Không tìm thấy khoa!" });
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Department deleted successfully: {ID}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting department: {ID}", id);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }
}
