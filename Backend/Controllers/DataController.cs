using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.ComponentModel.DataAnnotations;
using StudentManagement.Repositories;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataController> _logger;
    private readonly IDataRepository _dataRepository;

    public DataController(ApplicationDbContext context, ILogger<DataController> logger, IDataRepository dataRepository)
    {
        _context = context;
        _logger = logger;
        _dataRepository = dataRepository;
    }

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
                .Include(s => s.PermanentAddress)
                .Include(s => s.RegisteredAddress)
                .Include(s => s.TemporaryAddress)
                .Include(s => s.StudentStatus)
                .Select(s => new
                {
                    s.StudentId,
                    s.FullName,
                    s.DateOfBirth,
                    s.Gender,
                    Department = s.Department != null ? s.Department.Name : null,
                    SchoolYear = s.SchoolYear != null ? s.SchoolYear.Name : null,
                    StudyProgram = s.StudyProgram != null ? s.StudyProgram.Name : null,
                    PermanentAddress_HouseNumber = s.PermanentAddress.HouseNumber,
                    PermanentAddress_StreetName = s.PermanentAddress.StreetName,
                    PermanentAddress_Ward = s.PermanentAddress.Ward,
                    PermanentAddress_District = s.PermanentAddress.District,
                    PermanentAddress_Province = s.PermanentAddress.Province,
                    PermanentAddress_Country = s.PermanentAddress.Country,
                    RegisteredAddress_HouseNumber = s.RegisteredAddress.HouseNumber,
                    RegisteredAddress_StreetName = s.RegisteredAddress.StreetName,
                    RegisteredAddress_Ward = s.RegisteredAddress.Ward,
                    RegisteredAddress_District = s.RegisteredAddress.District,
                    RegisteredAddress_Province = s.RegisteredAddress.Province,
                    RegisteredAddress_Country = s.RegisteredAddress.Country,
                    TemporaryAddress_HouseNumber = s.TemporaryAddress.HouseNumber,
                    TemporaryAddress_StreetName = s.TemporaryAddress.StreetName,
                    TemporaryAddress_Ward = s.TemporaryAddress.Ward,
                    TemporaryAddress_District = s.TemporaryAddress.District,
                    TemporaryAddress_Province = s.TemporaryAddress.Province,
                    TemporaryAddress_Country = s.TemporaryAddress.Country,
                    s.Email,
                    s.PhoneNumber,
                    s.Nationality,
                    Identification_Type = s.Identification.IdentificationType,
                    Identification_Number = s.Identification.Number,
                    Identification_IssueDate = s.Identification.IssueDate,
                    Identification_ExpiryDate = s.Identification.ExpiryDate,
                    Identification_IssuedBy = s.Identification.IssuedBy,
                    Identification_HasChip = s.Identification.HasChip,
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
                .Include(s => s.PermanentAddress)
                .Include(s => s.RegisteredAddress)
                .Include(s => s.TemporaryAddress)
                .Include(s => s.StudentStatus)
                .Select(s => new
                {
                    s.StudentId,
                    s.FullName,
                    s.DateOfBirth,
                    s.Gender,
                    Department = s.Department != null ? s.Department.Name : null,
                    SchoolYear = s.SchoolYear != null ? s.SchoolYear.Name : null,
                    StudyProgram = s.StudyProgram != null ? s.StudyProgram.Name : null,
                    PermanentAddress_HouseNumber = s.PermanentAddress.HouseNumber,
                    PermanentAddress_StreetName = s.PermanentAddress.StreetName,
                    PermanentAddress_Ward = s.PermanentAddress.Ward,
                    PermanentAddress_District = s.PermanentAddress.District,
                    PermanentAddress_Province = s.PermanentAddress.Province,
                    PermanentAddress_Country = s.PermanentAddress.Country,
                    RegisteredAddress_HouseNumber = s.RegisteredAddress.HouseNumber,
                    RegisteredAddress_StreetName = s.RegisteredAddress.StreetName,
                    RegisteredAddress_Ward = s.RegisteredAddress.Ward,
                    RegisteredAddress_District = s.RegisteredAddress.District,
                    RegisteredAddress_Province = s.RegisteredAddress.Province,
                    RegisteredAddress_Country = s.RegisteredAddress.Country,
                    TemporaryAddress_HouseNumber = s.TemporaryAddress.HouseNumber,
                    TemporaryAddress_StreetName = s.TemporaryAddress.StreetName,
                    TemporaryAddress_Ward = s.TemporaryAddress.Ward,
                    TemporaryAddress_District = s.TemporaryAddress.District,
                    TemporaryAddress_Province = s.TemporaryAddress.Province,
                    TemporaryAddress_Country = s.TemporaryAddress.Country,
                    s.Email,
                    s.PhoneNumber,
                    s.Nationality,
                    Identification_Type = s.Identification.IdentificationType,
                    Identification_Number = s.Identification.Number,
                    Identification_IssueDate = s.Identification.IssueDate,
                    Identification_ExpiryDate = s.Identification.ExpiryDate,
                    Identification_IssuedBy = s.Identification.IssuedBy,
                    Identification_HasChip = s.Identification.HasChip,
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

    private async Task<Address> FindOrCreateAddressAsync(string houseNumber, string streetName, string ward, string district, string province, string country)
    {
        if (string.IsNullOrWhiteSpace(houseNumber) &&
            string.IsNullOrWhiteSpace(streetName) &&
            string.IsNullOrWhiteSpace(ward) &&
            string.IsNullOrWhiteSpace(district) &&
            string.IsNullOrWhiteSpace(province) &&
            string.IsNullOrWhiteSpace(country))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(houseNumber) ||
            string.IsNullOrWhiteSpace(streetName) ||
            string.IsNullOrWhiteSpace(ward) ||
            string.IsNullOrWhiteSpace(district) ||
            string.IsNullOrWhiteSpace(province) ||
            string.IsNullOrWhiteSpace(country))
        {
            throw new Exception("Địa chỉ không hợp lệ, cần điền tất cả các trường hoặc bỏ trống tất cả.");
        }

        var address = await _context.Addresses.FirstOrDefaultAsync(a =>
            a.HouseNumber == houseNumber &&
            a.StreetName == streetName &&
            a.Ward == ward &&
            a.District == district &&
            a.Province == province &&
            a.Country == country);

        if (address == null)
        {
            address = new Address
            {
                HouseNumber = houseNumber,
                StreetName = streetName,
                Ward = ward,
                District = district,
                Province = province,
                Country = country
            };
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
        }

        return address;
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
            var serviceProvider = HttpContext.RequestServices;
            var validationResults = new List<ValidationResult>();
            var newStudents = new List<Student>();

            foreach (var record in records)
            {
                var context = new ValidationContext(record, serviceProvider, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(record, context, results, true))
                {
                    validationResults.AddRange(results);
                    continue;
                }

                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == record.Department);
                if (department == null)
                {
                    return BadRequest(new { message = $"Department not found: {record.Department}" });
                }

                var schoolYear = await _context.SchoolYears.FirstOrDefaultAsync(sy => sy.Name == record.SchoolYear);
                if (schoolYear == null)
                {
                    return BadRequest(new { message = $"SchoolYear not found: {record.SchoolYear}" });
                }

                var studyProgram = await _context.StudyPrograms.FirstOrDefaultAsync(sp => sp.Name == record.StudyProgram);
                if (studyProgram == null)
                {
                    return BadRequest(new { message = $"StudyProgram not found: {record.StudyProgram}" });
                }

                var status = await _context.StudentStatuses.FirstOrDefaultAsync(st => st.Name == record.Status);
                if (status == null)
                {
                    return BadRequest(new { message = $"Status not found: {record.Status}" });
                }

                var PermanentAddress = await FindOrCreateAddressAsync(record.PermanentAddress_HouseNumber, record.PermanentAddress_StreetName, record.PermanentAddress_Ward, record.PermanentAddress_District, record.PermanentAddress_Province, record.PermanentAddress_Country);
                var RegisteredAddress = await FindOrCreateAddressAsync(record.RegisteredAddress_HouseNumber, record.RegisteredAddress_StreetName, record.RegisteredAddress_Ward, record.RegisteredAddress_District, record.RegisteredAddress_Province, record.RegisteredAddress_Country);
                var TemporaryAddress = await FindOrCreateAddressAsync(record.TemporaryAddress_HouseNumber, record.TemporaryAddress_StreetName, record.TemporaryAddress_Ward, record.TemporaryAddress_District, record.TemporaryAddress_Province, record.TemporaryAddress_Country);

                var identification = await _context.Identifications.FirstOrDefaultAsync(i => i.Number == record.Identification_Number);
                if (identification == null)
                {
                    identification = new Identification
                    {
                        IdentificationType = record.Identification_Type,
                        Number = record.Identification_Number,
                        IssueDate = record.Identification_IssueDate,
                        ExpiryDate = record.Identification_ExpiryDate,
                        IssuedBy = record.Identification_IssuedBy,
                        HasChip = record.Identification_HasChip,
                        IssuingCountry = record.Identification_IssuingCountry,
                        Notes = record.Identification_Notes
                    };
                    _context.Identifications.Add(identification);
                    await _context.SaveChangesAsync();
                }

                var student = new Student
                {
                    StudentId = record.StudentId,
                    FullName = record.FullName,
                    DateOfBirth = record.DateOfBirth,
                    Gender = record.Gender,
                    DepartmentId = department.Id,
                    SchoolYearId = schoolYear.Id,
                    StudyProgramId = studyProgram.Id,
                    StatusId = status.Id,
                    Email = record.Email,
                    Nationality = record.Nationality,
                    PhoneNumber = record.PhoneNumber,
                    PermanentAddressId = PermanentAddress?.Id ?? 0,
                    RegisteredAddressId = RegisteredAddress?.Id,
                    TemporaryAddressIdd = TemporaryAddress?.Id,
                    IdentificationId = identification.Id
                };

                newStudents.Add(student);
            }

            if (validationResults.Count > 0)
                return BadRequest(validationResults);

            var existingIds = _context.Students.Select(s => s.StudentId).ToHashSet();
            var uniqueStudents = newStudents.Where(s => !existingIds.Contains(s.StudentId)).ToList();

            if (!uniqueStudents.Any())
            {
                return BadRequest(new { message = "Tất cả dữ liệu đã tồn tại trong hệ thống!" });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Students.AddRangeAsync(uniqueStudents);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("CSV import successful.");
                return Ok(new { message = $"Import CSV thành công ({uniqueStudents.Count} bản ghi mới)." });
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

            var importedData = JsonConvert.DeserializeObject<List<StudentDto>>(json);
            if (importedData == null || !importedData.Any())
            {
                _logger.LogWarning("JSON file contains no valid data.");
                return BadRequest(new { message = "Dữ liệu JSON không hợp lệ." });
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
                    validationResults.AddRange(results);
                    continue;
                }

                var student = new Student
                {
                    StudentId = item.StudentId,
                    FullName = item.FullName,
                    DateOfBirth = item.DateOfBirth,
                    Gender = item.Gender,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    Nationality = item.Nationality,

                    Department = _context.Departments.FirstOrDefault(d => d.Name == item.Department),
                    SchoolYear = _context.SchoolYears.FirstOrDefault(y => y.Name == item.SchoolYear),
                    StudyProgram = _context.StudyPrograms.FirstOrDefault(p => p.Name == item.StudyProgram),
                    StudentStatus = _context.StudentStatuses.FirstOrDefault(st => st.Name == item.Status),

                    PermanentAddress = new Address
                    {
                        HouseNumber = item.PermanentAddress_HouseNumber,
                        StreetName = item.PermanentAddress_StreetName,
                        Ward = item.PermanentAddress_Ward,
                        District = item.PermanentAddress_District,
                        Province = item.PermanentAddress_Province,
                        Country = item.PermanentAddress_Country
                    },
                    RegisteredAddress = item.RegisteredAddress_HouseNumber != null ? new Address
                    {
                        HouseNumber = item.RegisteredAddress_HouseNumber,
                        StreetName = item.RegisteredAddress_StreetName,
                        Ward = item.RegisteredAddress_Ward,
                        District = item.RegisteredAddress_District,
                        Province = item.RegisteredAddress_Province,
                        Country = item.RegisteredAddress_Country
                    } : null,
                    TemporaryAddress = item.TemporaryAddress_HouseNumber != null ? new Address
                    {
                        HouseNumber = item.TemporaryAddress_HouseNumber,
                        StreetName = item.TemporaryAddress_StreetName,
                        Ward = item.TemporaryAddress_Ward,
                        District = item.TemporaryAddress_District,
                        Province = item.TemporaryAddress_Province,
                        Country = item.TemporaryAddress_Country
                    } : null,

                    Identification = new Identification
                    {
                        IdentificationType = item.Identification_Type,
                        Number = item.Identification_Number,
                        IssueDate = item.Identification_IssueDate,
                        ExpiryDate = item.Identification_ExpiryDate,
                        IssuedBy = item.Identification_IssuedBy,
                        IssuingCountry = item.Identification_IssuingCountry,
                        Notes = item.Identification_Notes
                    }
                };

                _logger.LogInformation($"New student: {student.GetType()} value: {student}");
                newStudents.Add(student);
            }

            if (validationResults.Count > 0)
                return BadRequest(validationResults);

            var existingIds = _context.Students.Select(s => s.StudentId).ToHashSet();
            newStudents = newStudents.Where(s => !existingIds.Contains(s.StudentId)).ToList();

            if (!newStudents.Any())
            {
                return BadRequest(new { message = "Tất cả dữ liệu đã tồn tại trong hệ thống!" });
            }

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