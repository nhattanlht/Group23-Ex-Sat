using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataController> _logger;

    public DataController(ApplicationDbContext context, ILogger<DataController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // 1. Export JSON (Download file)
    [HttpGet("export/json")]
    public IActionResult ExportJson()
    {
        _logger.LogInformation("Exporting students data to JSON.");
        try
        {
            var data = _context.Students.AsNoTracking().ToList();
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var byteArray = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            _logger.LogInformation("JSON file generated successfully.");
            return File(stream, "application/json", "students.json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting JSON file.");
            return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
        }
    }

    // 2. Export CSV (Download file)
    [HttpGet("export/csv")]
    public IActionResult ExportCsv()
    {
        _logger.LogInformation("Exporting students data to CSV.");
        try
        {
            var data = _context.Students.AsNoTracking().ToList();
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ","
            };

            using var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteRecords(data);
                writer.Flush();
            }

            var outputStream = new MemoryStream(memoryStream.ToArray());

            _logger.LogInformation("CSV file generated successfully.");
            return File(outputStream, "text/csv", "students.csv");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting CSV file.");
            return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
        }
    }

    // 3. Import CSV (Thêm kiểm tra trùng lặp & Transaction)
    [HttpPost("import/csv")]
    public async Task<IActionResult> ImportCsv([FromForm] IFormFile file)
    {
        _logger.LogInformation("Importing CSV file.");
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Invalid CSV file.");
            return BadRequest(new { message = "File không hợp lệ." });
        }

        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            var records = csv.GetRecords<Student>().ToList();
            if (!records.Any())
            {
                _logger.LogWarning("CSV file contains no valid data.");
                return BadRequest(new { message = "Dữ liệu CSV không hợp lệ." });
            }

            // Kiểm tra trùng lặp trước khi thêm vào DB
            var existingIds = _context.Students.Select(s => s.MSSV).ToHashSet();
            var newStudents = records.Where(s => !existingIds.Contains(s.MSSV)).ToList();

            if (!newStudents.Any())
            {
                return BadRequest(new { message = "Tất cả dữ liệu đã tồn tại trong hệ thống!" });
            }

            // Sử dụng transaction để đảm bảo toàn vẹn dữ liệu
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Students.AddRangeAsync(newStudents);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("CSV import successful.");
                return Ok(new { message = $"Import CSV thành công ({newStudents.Count} bản ghi mới)." });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing CSV.");
            return StatusCode(500, new { message = "Lỗi khi import CSV", error = ex.Message });
        }
    }

    // 4. Import JSON (Thêm kiểm tra trùng lặp & Transaction)
    [HttpPost("import/json")]
    public async Task<IActionResult> ImportJson([FromForm] IFormFile file)
    {
        _logger.LogInformation("Importing JSON file.");
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Invalid JSON file.");
            return BadRequest(new { message = "File không hợp lệ." });
        }

        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var students = JsonConvert.DeserializeObject<List<Student>>(json);

            if (students == null || !students.Any())
            {
                _logger.LogWarning("JSON file contains no valid data.");
                return BadRequest(new { message = "Dữ liệu JSON không hợp lệ." });
            }

            // Kiểm tra trùng lặp trước khi thêm vào DB
            var existingIds = _context.Students.Select(s => s.MSSV).ToHashSet();
            var newStudents = students.Where(s => !existingIds.Contains(s.MSSV)).ToList();

            if (!newStudents.Any())
            {
                return BadRequest(new { message = "Tất cả dữ liệu đã tồn tại trong hệ thống!" });
            }

            // Sử dụng transaction để đảm bảo toàn vẹn dữ liệu
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Students.AddRangeAsync(newStudents);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("JSON import successful.");
                return Ok(new { message = $"Import JSON thành công ({newStudents.Count} bản ghi mới)." });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing JSON.");
            return StatusCode(500, new { message = "Lỗi khi import JSON", error = ex.Message });
        }
    }
}
