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
            var data = _context.Students
                .Include(s => s.Department)
                .Include(s => s.SchoolYear)
                .Include(s => s.StudyProgram)
                .Include(s => s.DiaChiNhanThu)
                .Include(s => s.DiaChiThuongTru)
                .Include(s => s.DiaChiTamTru)
                .Include(s => s.StudentStatus)
                .Select(s => new
                {
                    s.MSSV,
                    s.HoTen,
                    s.NgaySinh,
                    s.GioiTinh,
                    Department = s.Department != null ? s.Department.Name : null,
                    SchoolYear = s.SchoolYear != null ? s.SchoolYear.Name : null,
                    StudyProgram = s.StudyProgram != null ? s.StudyProgram.Name : null,

                    AddressNhanThu_HouseNumber = s.DiaChiNhanThu.HouseNumber,
                    AddressNhanThu_StreetName = s.DiaChiNhanThu.StreetName,
                    AddressNhanThu_Ward = s.DiaChiNhanThu.Ward,
                    AddressNhanThu_District = s.DiaChiNhanThu.District,
                    AddressNhanThu_Country = s.DiaChiNhanThu.Country,

                    AddressThuongTru_HouseNumber = s.DiaChiThuongTru.HouseNumber,
                    AddressThuongTru_StreetName = s.DiaChiThuongTru.StreetName,
                    AddressThuongTru_Ward = s.DiaChiThuongTru.Ward,
                    AddressThuongTru_District = s.DiaChiThuongTru.District,
                    AddressThuongTru_Country = s.DiaChiThuongTru.Country,

                    AddressTamTru_HouseNumber = s.DiaChiTamTru.HouseNumber,
                    AddressTamTru_StreetName = s.DiaChiTamTru.StreetName,
                    AddressTamTru_Ward = s.DiaChiTamTru.Ward,
                    AddressTamTru_District = s.DiaChiTamTru.District,
                    AddressTamTru_Country = s.DiaChiTamTru.Country,

                    s.Email,
                    s.SoDienThoai,
                    s.QuocTich,

                    Identification_Type = s.Identification.IdentificationType,
                    Identification_Number = s.Identification.Number,
                    Identification_IssueDate = s.Identification.IssueDate,
                    Identification_ExpiryDate = s.Identification.ExpiryDate,
                    Identification_IssuedBy = s.Identification.IssuedBy,
                    Identification_IssuingCountry = s.Identification.IssuingCountry,
                    Identification_Notes = s.Identification.Notes,

                    Status = s.StudentStatus.Name
                })
                .AsNoTracking().ToList();
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
            var data = _context.Students
                .Include(s => s.Department)
                .Include(s => s.SchoolYear)
                .Include(s => s.StudyProgram)
                .Include(s => s.DiaChiNhanThu)
                .Include(s => s.DiaChiThuongTru)
                .Include(s => s.DiaChiTamTru)
                .Include(s => s.StudentStatus)
                .Select(s => new
                {
                    s.MSSV,
                    s.HoTen,
                    s.NgaySinh,
                    s.GioiTinh,
                    Department = s.Department != null ? s.Department.Name : null,
                    SchoolYear = s.SchoolYear != null ? s.SchoolYear.Name : null,
                    StudyProgram = s.StudyProgram != null ? s.StudyProgram.Name : null,

                    AddressNhanThu_HouseNumber = s.DiaChiNhanThu.HouseNumber,
                    AddressNhanThu_StreetName = s.DiaChiNhanThu.StreetName,
                    AddressNhanThu_Ward = s.DiaChiNhanThu.Ward,
                    AddressNhanThu_District = s.DiaChiNhanThu.District,
                    AddressNhanThu_Country = s.DiaChiNhanThu.Country,

                    AddressThuongTru_HouseNumber = s.DiaChiThuongTru.HouseNumber,
                    AddressThuongTru_StreetName = s.DiaChiThuongTru.StreetName,
                    AddressThuongTru_Ward = s.DiaChiThuongTru.Ward,
                    AddressThuongTru_District = s.DiaChiThuongTru.District,
                    AddressThuongTru_Country = s.DiaChiThuongTru.Country,

                    AddressTamTru_HouseNumber = s.DiaChiTamTru.HouseNumber,
                    AddressTamTru_StreetName = s.DiaChiTamTru.StreetName,
                    AddressTamTru_Ward = s.DiaChiTamTru.Ward,
                    AddressTamTru_District = s.DiaChiTamTru.District,
                    AddressTamTru_Country = s.DiaChiTamTru.Country,

                    s.Email,
                    s.SoDienThoai,
                    s.QuocTich,

                    Identification_Type = s.Identification.IdentificationType,
                    Identification_Number = s.Identification.Number,
                    Identification_IssueDate = s.Identification.IssueDate,
                    Identification_ExpiryDate = s.Identification.ExpiryDate,
                    Identification_IssuedBy = s.Identification.IssuedBy,
                    Identification_IssuingCountry = s.Identification.IssuingCountry,
                    Identification_Notes = s.Identification.Notes,

                    Status = s.StudentStatus.Name
                })
                .AsNoTracking().ToList();

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

    // 3. Import CSV

    // 4. Import JSON
    [Route("api/[controller]")]
    [HttpPost("import/json")]
    public async Task<IActionResult> ImportJson([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "File is empty or not provided." });
        }

        try
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var jsonContent = await stream.ReadToEndAsync();
                var students = JsonConvert.DeserializeObject<List<Student>>(jsonContent);

                if (students == null)
                {
                    return BadRequest(new { message = "Invalid JSON format." });
                }

                // Validate and add students to the database
                foreach (var student in students)
                {
                    // Check if the student already exists
                    var existingStudent = await _context.Students.FindAsync(student.MSSV);
                    if (existingStudent != null)
                    {
                        // Update existing student
                        _context.Entry(existingStudent).CurrentValues.SetValues(student);
                    }
                    else
                    {
                        // Add new student
                        _context.Students.Add(student);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Students imported successfully.", count = students.Count });
            }
        }
        catch (JsonSerializationException ex)
        {
            return BadRequest(new { message = "Error parsing JSON.", details = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while importing students.", details = ex.Message });
        }
    }

}

