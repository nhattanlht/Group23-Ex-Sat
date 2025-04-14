using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly DataService _dataService;
    private readonly ILogger<DataController> _logger;

    public DataController(DataService dataService, ILogger<DataController> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }

    [HttpGet("export/json")]
    public async Task<IActionResult> ExportJson()
    {
        _logger.LogInformation("Exporting students data to JSON.");
        try
        {
            var students = await _dataService.GetAllStudentsAsync();
            var json = JsonConvert.SerializeObject(students, Formatting.Indented);
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

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv()
    {
        _logger.LogInformation("Exporting students data to CSV.");
        try
        {
            var students = await _dataService.GetAllStudentsAsync();

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ","
            };

            var memoryStream = new MemoryStream(); // Do not use 'using' here
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true)) // Keep the stream open
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.Context.RegisterClassMap<StudentMap>(); // Use the same mapping for export
                csv.WriteRecords(students); // Write the student records to the CSV
                writer.Flush(); // Ensure all data is written to the memory stream
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

    [HttpPost("import/json")]
    public async Task<IActionResult> ImportJson([FromForm] IFormFile file)
    {
        var (isSuccess, message, count) = await _dataService.ImportStudentsFromJsonAsync(file, _logger);

        if (!isSuccess)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message = $"{message} ({count} bản ghi mới)." });
    }

    [HttpPost("import/csv")]
    public async Task<IActionResult> ImportCsv([FromForm] IFormFile file)
    {
        var (isSuccess, message, count) = await _dataService.ImportStudentsFromCsvAsync(file, _logger);

        if (!isSuccess)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message = $"{message} ({count} bản ghi mới)." });
    }
}