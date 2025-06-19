using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using StudentManagement;
using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.Services;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataController> _logger;
    private readonly IDataService _dataService;

    private readonly IDataRepository _dataRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public DataController(
        ApplicationDbContext context,
        ILogger<DataController> logger,
        IDataService dataService,
        IDataRepository dataRepository,
        IStringLocalizer<SharedResource> localizer
    )
    {
        _context = context;
        _logger = logger;
        _dataService = dataService;
        _dataRepository = dataRepository;
        _localizer = localizer;
    }

    [HttpGet("export/json")]
    public async Task<IActionResult> ExportJson()
    {
        _logger.LogInformation("Exporting students data to JSON.");
        try
        {
            var data = await _dataService.GetAllStudentsAsync();

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var byteArray = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            _logger.LogInformation("JSON file generated successfully.");
            return File(stream, "application/json", "students.json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting JSON file.");
            return StatusCode(
                500,
                new { message = _localizer["InternalServerError"].Value, error = ex.Message }
            );
        }
    }

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv()
    {
        _logger.LogInformation("Exporting students data to CSV.");
        try
        {
            var data = await  _dataService.GetAllStudentsAsync();

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ",",
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
            return StatusCode(
                500,
                new { message = _localizer["InternalServerError"].Value, error = ex.Message }
            );
        }
    }

    [HttpPost("import/csv")]
    public async Task<IActionResult> ImportCsv([FromForm] IFormFile file)
    {
        _logger.LogInformation("Importing CSV file.");
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Invalid CSV file.");
            return BadRequest(new { message = _localizer["InvalidFile"].Value, status = "Error" });
        }

        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(
                reader,
                new CsvConfiguration(CultureInfo.InvariantCulture)
            );

            csv.Context.RegisterClassMap<StudentMap>();
            var records = csv.GetRecords<StudentDto>().ToList();

            if (!records.Any())
            {
                _logger.LogWarning("CSV file contains no valid data.");
                return BadRequest(
                    new { message = _localizer["InvalidCsvData"].Value, status = "Error" }
                );
            }
            var serviceProvider = HttpContext.RequestServices;
            var validationResults = new List<ValidationResult>();
            var newStudents = new List<Student>();

            foreach (var record in records)
            {
                var context = new ValidationContext(record, serviceProvider, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(record, context, results, true))
                {
                    foreach (var result in results)
                    {
                        var localizer = serviceProvider.GetService<
                            IStringLocalizer<ValidationMessages>
                        >();
                        var localizedMessage = localizer[result.ErrorMessage];
                        // Ghi lại lỗi đã dịch
                        validationResults.Add(
                            new ValidationResult(localizedMessage, result.MemberNames)
                        );
                    }
                    continue;
                }
            }
            if (validationResults.Count > 0)
                return BadRequest(validationResults);

            var (success, message, importedCount) = await _dataService.ImportStudentsFromCsvAsync(
                file,
                _logger
            );
            if (success)
            {
                _logger.LogInformation("CSV import successful.");
                return Ok(
                    new
                    {
                        data = importedCount,
                        message,
                        status = "Success",
                    }
                );
            }
            else
            {
                _logger.LogWarning("CSV import failed.");
                return BadRequest(new { message, status = "Error" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing CSV.");
            return StatusCode(
                500,
                new
                {
                    message = _localizer["ImportCsvError"].Value,
                    error = ex.Message,
                    status = "Error",
                }
            );
        }
    }

    [HttpPost("import/json")]
    public async Task<IActionResult> ImportJson([FromForm] IFormFile file)
    {
        _logger.LogInformation("Importing JSON file.");
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Invalid JSON file.");
            return BadRequest(new { message = _localizer["InvalidFile"].Value, status = "Error" });
        }

        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var importedData = JsonConvert.DeserializeObject<List<StudentDto>>(json);
            if (importedData == null || !importedData.Any())
            {
                _logger.LogWarning("JSON file contains no valid data.");
                return BadRequest(
                    new { message = _localizer["InvalidJsonData"].Value, status = "Error" }
                );
            }

            var validationResults = new List<ValidationResult>();
            var newStudents = new List<Student>();
            var serviceProvider = HttpContext.RequestServices;

            foreach (var item in importedData)
            {
                var context = new ValidationContext(item, serviceProvider, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(item, context, results, true))
                {
                    foreach (var result in results)
                    {
                        var localizer = serviceProvider.GetService<
                            IStringLocalizer<ValidationMessages>
                        >();
                        var localizedMessage = localizer[result.ErrorMessage];
                        // Ghi lại lỗi đã dịch
                        validationResults.Add(
                            new ValidationResult(localizedMessage, result.MemberNames)
                        );
                    }
                    continue;
                }
            }

            if (validationResults.Count > 0)
                return BadRequest(validationResults);

            var (success, message, importedCount) = await _dataService.ImportStudentsFromJsonAsync(
                file,
                _logger
            );
            if (success)
            {
                _logger.LogInformation("JSON import successful.");
                return Ok(
                    new
                    {
                        message,
                        status = "Success",
                        data = importedCount,
                    }
                );
            }
            else
            {
                _logger.LogWarning("JSON import failed.");
                return BadRequest(new { message, status = "Error" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing JSON.");
            return StatusCode(
                500,
                new
                {
                    message = _localizer["ImportJsonError", ""].Value,
                    error = ex.Message,
                    status = "Error",
                }
            );
        }
    }

    public async Task<IActionResult> GetData(int id)
    {
        var data = await _dataRepository.GetDataByIdAsync(id);
        if (data == null)
        {
            return NotFound(new { message = "Data not found" });
        }

        return Ok(data);
    }
}
