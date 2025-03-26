using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.ComponentModel.DataAnnotations;
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
                .Include(s => s.Department) // Lấy thông tin khoa
                .Include(s => s.SchoolYear) // Lấy thông tin năm học
                .Include(s => s.StudyProgram) // Lấy thông tin chương trình học
                .Include(s => s.DiaChiNhanThu) // Lấy địa chỉ nhận thư
                .Include(s => s.DiaChiThuongTru) // Lấy địa chỉ thường trú
                .Include(s => s.DiaChiTamTru) // Lấy địa chỉ tạm trú
                .Include(s => s.StudentStatus) // Lấy trạng thái sinh viên
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
                    AddressNhanThu_Province = s.DiaChiNhanThu.Province,
                    AddressNhanThu_Country = s.DiaChiNhanThu.Country,

                    AddressThuongTru_HouseNumber = s.DiaChiThuongTru.HouseNumber,
                    AddressThuongTru_StreetName = s.DiaChiThuongTru.StreetName,
                    AddressThuongTru_Ward = s.DiaChiThuongTru.Ward,
                    AddressThuongTru_District = s.DiaChiThuongTru.District,
                    AddressThuongTru_Province = s.DiaChiThuongTru.Province,
                    AddressThuongTru_Country = s.DiaChiThuongTru.Country,

                    AddressTamTru_HouseNumber = s.DiaChiTamTru.HouseNumber,
                    AddressTamTru_StreetName = s.DiaChiTamTru.StreetName,
                    AddressTamTru_Ward = s.DiaChiTamTru.Ward,
                    AddressTamTru_District = s.DiaChiTamTru.District,
                    AddressTamTru_Province = s.DiaChiTamTru.Province,
                    AddressTamTru_Country = s.DiaChiTamTru.Country,

                    s.Email,
                    s.SoDienThoai,
                    s.QuocTich,

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
                    AddressNhanThu_Province = s.DiaChiNhanThu.Province,
                    AddressNhanThu_Country = s.DiaChiNhanThu.Country,

                    AddressThuongTru_HouseNumber = s.DiaChiThuongTru.HouseNumber,
                    AddressThuongTru_StreetName = s.DiaChiThuongTru.StreetName,
                    AddressThuongTru_Ward = s.DiaChiThuongTru.Ward,
                    AddressThuongTru_District = s.DiaChiThuongTru.District,
                    AddressThuongTru_Province = s.DiaChiThuongTru.Province,
                    AddressThuongTru_Country = s.DiaChiThuongTru.Country,

                    AddressTamTru_HouseNumber = s.DiaChiTamTru.HouseNumber,
                    AddressTamTru_StreetName = s.DiaChiTamTru.StreetName,
                    AddressTamTru_Ward = s.DiaChiTamTru.Ward,
                    AddressTamTru_District = s.DiaChiTamTru.District,
                    AddressTamTru_Province = s.DiaChiTamTru.Province,
                    AddressTamTru_Country = s.DiaChiTamTru.Country,

                    s.Email,
                    s.SoDienThoai,
                    s.QuocTich,

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


    // 3. Import CSV (Thêm kiểm tra trùng lặp & Transaction)
    private async Task<Address> FindOrCreateAddressAsync(string houseNumber, string streetName, string ward, string district, string province, string country)
    {
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

                // 🔹 Convert Department Name to ID
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == record.Department);
                if (department == null)
                {
                    return BadRequest(new { message = $"Department not found: {record.Department}" });
                }

                // 🔹 Convert School Year Name to ID
                var schoolYear = await _context.SchoolYears.FirstOrDefaultAsync(sy => sy.Name == record.SchoolYear);
                if (schoolYear == null)
                {
                    return BadRequest(new { message = $"SchoolYear not found: {record.SchoolYear}" });
                }

                // 🔹 Convert Study Program Name to ID
                var studyProgram = await _context.StudyPrograms.FirstOrDefaultAsync(sp => sp.Name == record.StudyProgram);
                if (studyProgram == null)
                {
                    return BadRequest(new { message = $"StudyProgram not found: {record.StudyProgram}" });
                }

                // 🔹 Convert Status Name to ID
                var status = await _context.StudentStatuses.FirstOrDefaultAsync(st => st.Name == record.Status);
                if (status == null)
                {
                    return BadRequest(new { message = $"Status not found: {record.Status}" });
                }

                // 🔹 Find or create Addresses
                var diaChiNhanThu = await FindOrCreateAddressAsync(record.AddressNhanThu_HouseNumber, record.AddressNhanThu_StreetName, record.AddressNhanThu_Ward, record.AddressNhanThu_District, record.AddressNhanThu_Province, record.AddressNhanThu_Country);
                var diaChiThuongTru = await FindOrCreateAddressAsync(record.AddressThuongTru_HouseNumber, record.AddressThuongTru_StreetName, record.AddressThuongTru_Ward, record.AddressThuongTru_District, record.AddressThuongTru_Province, record.AddressThuongTru_Country);
                var diaChiTamTru = await FindOrCreateAddressAsync(record.AddressTamTru_HouseNumber, record.AddressTamTru_StreetName, record.AddressTamTru_Ward, record.AddressTamTru_District, record.AddressTamTru_Province, record.AddressTamTru_Country);

                // 🔹 Find or create Identification
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

                // 🔹 Create Student object
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
                    DiaChiNhanThuId = diaChiNhanThu?.Id ?? 0,
                    DiaChiThuongTruId = diaChiThuongTru?.Id,
                    DiaChiTamTruId = diaChiTamTru?.Id,
                    IdentificationId = identification.Id
                };

                newStudents.Add(student);
            }

            if (validationResults.Count > 0)
            return BadRequest(validationResults);

            // 🔹 Check for duplicates before inserting
            var existingIds = _context.Students.Select(s => s.MSSV).ToHashSet();
            var uniqueStudents = newStudents.Where(s => !existingIds.Contains(s.MSSV)).ToList();

            if (!uniqueStudents.Any())
            {
                return BadRequest(new { message = "Tất cả dữ liệu đã tồn tại trong hệ thống!" });
            }

            // 🔹 Use transaction for data integrity
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

            var importedData = JsonConvert.DeserializeObject<List<StudentDto>>(json);
            if (importedData == null || !importedData.Any())
            {
                _logger.LogWarning("JSON file contains no valid data.");
                return BadRequest(new { message = "Dữ liệu JSON không hợp lệ." });
            }

            // Danh sách sinh viên để thêm vào DB
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
                    MSSV = item.MSSV,
                    HoTen = item.HoTen,
                    NgaySinh = item.NgaySinh,
                    GioiTinh = item.GioiTinh,
                    Email = item.Email,
                    SoDienThoai = item.SoDienThoai,
                    QuocTich = item.QuocTich,

                    // Liên kết với bảng ngoại
                    Department = _context.Departments.FirstOrDefault(d => d.Name == item.Department),
                    SchoolYear = _context.SchoolYears.FirstOrDefault(y => y.Name == item.SchoolYear),
                    StudyProgram = _context.StudyPrograms.FirstOrDefault(p => p.Name == item.StudyProgram),
                    StudentStatus = _context.StudentStatuses.FirstOrDefault(st => st.Name == item.Status),


                    // Địa chỉ
                    DiaChiNhanThu = new Address
                    {
                        HouseNumber = item.AddressNhanThu_HouseNumber,
                        StreetName = item.AddressNhanThu_StreetName,
                        Ward = item.AddressNhanThu_Ward,
                        District = item.AddressNhanThu_District,
                        Province = item.AddressNhanThu_Province,
                        Country = item.AddressNhanThu_Country
                    },
                    DiaChiThuongTru = item.AddressThuongTru_HouseNumber != null ? new Address
                    {
                        HouseNumber = item.AddressThuongTru_HouseNumber,
                        StreetName = item.AddressThuongTru_StreetName,
                        Ward = item.AddressThuongTru_Ward,
                        District = item.AddressThuongTru_District,
                        Province = item.AddressThuongTru_Province,
                        Country = item.AddressThuongTru_Country
                    } : null,
                    DiaChiTamTru = item.AddressTamTru_HouseNumber != null ? new Address
                    {
                        HouseNumber = item.AddressTamTru_HouseNumber,
                        StreetName = item.AddressTamTru_StreetName,
                        Ward = item.AddressTamTru_Ward,
                        District = item.AddressTamTru_District,
                        Province = item.AddressTamTru_Province,
                        Country = item.AddressTamTru_Country
                    } : null,

                    // Thông tin định danh
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

            // Kiểm tra trùng lặp
            var existingIds = _context.Students.Select(s => s.MSSV).ToHashSet();
            newStudents = newStudents.Where(s => !existingIds.Contains(s.MSSV)).ToList();

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
