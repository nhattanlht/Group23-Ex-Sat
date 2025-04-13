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

            var records = JsonConvert.DeserializeObject<List<StudentDto>>(json);
            if (records == null || !records.Any())
            {
                _logger.LogWarning("JSON file contains no valid data.");
                return BadRequest(new { message = "Dữ liệu JSON không hợp lệ." });
            }

            var newStudents = new List<Student>();

            foreach (var record in records)
            {
                var department = await _dataService.GetDepartmentByNameAsync(record.Department);
                var schoolYear = await _dataService.GetSchoolYearByNameAsync(record.SchoolYear);
                var studyProgram = await _dataService.GetStudyProgramByNameAsync(record.StudyProgram);
                var status = await _dataService.GetStudentStatusByNameAsync(record.Status);

                var diaChiNhanThu = await _dataService.FindOrCreateAddressAsync(
                    record.AddressNhanThu_HouseNumber,
                    record.AddressNhanThu_StreetName,
                    record.AddressNhanThu_Ward,
                    record.AddressNhanThu_District,
                    record.AddressNhanThu_Province,
                    record.AddressNhanThu_Country);

                var diaChiThuongTru = await _dataService.FindOrCreateAddressAsync(
                    record.AddressThuongTru_HouseNumber,
                    record.AddressThuongTru_StreetName,
                    record.AddressThuongTru_Ward,
                    record.AddressThuongTru_District,
                    record.AddressThuongTru_Province,
                    record.AddressThuongTru_Country);
                    
                var diaChiTamTru = await _dataService.FindOrCreateAddressAsync(
                    record.AddressTamTru_HouseNumber,
                    record.AddressTamTru_StreetName,
                    record.AddressTamTru_Ward,
                    record.AddressTamTru_District,
                    record.AddressTamTru_Province,
                    record.AddressTamTru_Country);

                var identification = await _dataService.FindOrCreateIdentificationAsync(new Identification
                {
                    IdentificationType = record.Identification_Type,
                    Number = record.Identification_Number,
                    IssueDate = record.Identification_IssueDate,
                    ExpiryDate = record.Identification_ExpiryDate,
                    IssuedBy = record.Identification_IssuedBy,
                    HasChip = record.Identification_HasChip,
                    IssuingCountry = record.Identification_IssuingCountry,
                    Notes = record.Identification_Notes
                });

                var student = new Student
                {
                    MSSV = record.MSSV,
                    HoTen = record.HoTen,
                    NgaySinh = record.NgaySinh,
                    GioiTinh = record.GioiTinh,
                    DepartmentId = department.Id,
                    SchoolYearId = schoolYear.Id,
                    StudyProgramId = studyProgram.Id,
                    StatusId = status.Id,
                    Email = record.Email,
                    QuocTich = record.QuocTich,
                    SoDienThoai = record.SoDienThoai,
                    DiaChiNhanThuId = diaChiNhanThu.Id,
                    DiaChiThuongTruId = diaChiThuongTru?.Id ?? null,
                    DiaChiTamTruId = diaChiTamTru?.Id ?? null,
                    IdentificationId = identification.Id
                };

                newStudents.Add(student);
            }

            var uniqueStudents = await _dataService.FilterDuplicateStudentsAsync(newStudents);

            if (!uniqueStudents.Any())
            {
                return BadRequest(new { message = "Tất cả dữ liệu đã tồn tại trong hệ thống!" });
            }

            await _dataService.ImportStudentsAsync(uniqueStudents);

            _logger.LogInformation("JSON import successful.");
            return Ok(new { message = $"Import JSON thành công ({uniqueStudents.Count} bản ghi mới)." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing JSON.");
            return StatusCode(500, new { message = "Lỗi khi import JSON", error = ex.Message });
        }
    }
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

            csv.Context.RegisterClassMap<StudentMap>();
            var records = csv.GetRecords<StudentDto>().ToList();

            if (!records.Any())
            {
                _logger.LogWarning("CSV file contains no valid data.");
                return BadRequest(new { message = "Dữ liệu CSV không hợp lệ." });
            }

            var newStudents = new List<Student>();

            foreach (var record in records)
            {
                var department = await _dataService.GetDepartmentByNameAsync(record.Department);
                var schoolYear = await _dataService.GetSchoolYearByNameAsync(record.SchoolYear);
                var studyProgram = await _dataService.GetStudyProgramByNameAsync(record.StudyProgram);
                var status = await _dataService.GetStudentStatusByNameAsync(record.Status);

                var diaChiNhanThu = await _dataService.FindOrCreateAddressAsync(
                    record.AddressNhanThu_HouseNumber,
                    record.AddressNhanThu_StreetName,
                    record.AddressNhanThu_Ward,
                    record.AddressNhanThu_District,
                    record.AddressNhanThu_Province,
                    record.AddressNhanThu_Country);

                var diaChiThuongTru = await _dataService.FindOrCreateAddressAsync(
                    record.AddressThuongTru_HouseNumber,
                    record.AddressThuongTru_StreetName,
                    record.AddressThuongTru_Ward,
                    record.AddressThuongTru_District,
                    record.AddressThuongTru_Province,
                    record.AddressThuongTru_Country);

                var diaChiTamTru = await _dataService.FindOrCreateAddressAsync(
                    record.AddressTamTru_HouseNumber,
                    record.AddressTamTru_StreetName,
                    record.AddressTamTru_Ward,
                    record.AddressTamTru_District,
                    record.AddressTamTru_Province,
                    record.AddressTamTru_Country);

                var identification = await _dataService.FindOrCreateIdentificationAsync(new Identification
                {
                    IdentificationType = record.Identification_Type,
                    Number = record.Identification_Number,
                    IssueDate = record.Identification_IssueDate,
                    ExpiryDate = record.Identification_ExpiryDate,
                    IssuedBy = record.Identification_IssuedBy,
                    HasChip = record.Identification_HasChip,
                    IssuingCountry = record.Identification_IssuingCountry,
                    Notes = record.Identification_Notes
                });

                var student = new Student
                {
                    MSSV = record.MSSV,
                    HoTen = record.HoTen,
                    NgaySinh = record.NgaySinh,
                    GioiTinh = record.GioiTinh,
                    DepartmentId = department.Id,
                    SchoolYearId = schoolYear.Id,
                    StudyProgramId = studyProgram.Id,
                    StatusId = status.Id,
                    Email = record.Email,
                    QuocTich = record.QuocTich,
                    SoDienThoai = record.SoDienThoai,
                    DiaChiNhanThuId = diaChiNhanThu.Id,
                    DiaChiThuongTruId = diaChiThuongTru?.Id ?? null,
                    DiaChiTamTruId = diaChiTamTru?.Id ?? null,
                    IdentificationId = identification.Id
                };

                newStudents.Add(student);
            }

            var uniqueStudents = await _dataService.FilterDuplicateStudentsAsync(newStudents);

            if (!uniqueStudents.Any())
            {
                return BadRequest(new { message = "Tất cả dữ liệu đã tồn tại trong hệ thống!" });
            }

            await _dataService.ImportStudentsAsync(uniqueStudents);

            _logger.LogInformation("CSV import successful.");
            return Ok(new { message = $"Import CSV thành công ({uniqueStudents.Count} bản ghi mới)." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing CSV.");
            return StatusCode(500, new { message = "Lỗi khi import CSV", error = ex.Message });
        }
    }
}