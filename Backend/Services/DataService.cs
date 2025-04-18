using StudentManagement.Models;
using StudentManagement.DTOs;
using StudentManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace StudentManagement.Services
{
    public class DataService: IDataService
    {
        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            return await _repository.GetAllStudentsAsync();
        }

        public async Task<Address> FindOrCreateAddressAsync(string? houseNumber, string? streetName, string? ward, string? district, string? province, string? country)
        {
            if (string.IsNullOrEmpty(houseNumber) || string.IsNullOrEmpty(streetName) || string.IsNullOrEmpty(ward) ||
                string.IsNullOrEmpty(district) || string.IsNullOrEmpty(province) || string.IsNullOrEmpty(country))
            {
                throw new ArgumentNullException("Address fields cannot be null or empty.");
            }

            // Case 1: All fields are empty → No address needed
            if (string.IsNullOrWhiteSpace(houseNumber) &&
                string.IsNullOrWhiteSpace(streetName) &&
                string.IsNullOrWhiteSpace(ward) &&
                string.IsNullOrWhiteSpace(district) &&
                string.IsNullOrWhiteSpace(province) &&
                string.IsNullOrWhiteSpace(country))
            {
                return null; // No address needed
            }

            // Case 2: Some fields are missing → This is an error
            if (string.IsNullOrWhiteSpace(houseNumber) ||
                string.IsNullOrWhiteSpace(streetName) ||
                string.IsNullOrWhiteSpace(ward) ||
                string.IsNullOrWhiteSpace(district) ||
                string.IsNullOrWhiteSpace(province) ||
                string.IsNullOrWhiteSpace(country))
            {
                throw new Exception("Địa chỉ không hợp lệ, cần điền tất cả các trường hoặc bỏ trống tất cả.");
            }

            // Case 3: Find or create the address
            return await _repository.FindOrCreateAddressAsync(new Address
            {
                HouseNumber = houseNumber,
                StreetName = streetName,
                Ward = ward,
                District = district,
                Province = province,
                Country = country
            });
        }

        public async Task<Department> GetDepartmentByNameAsync(string departmentName)
        {
            var department = await _repository.GetDepartmentByNameAsync(departmentName);
            if (department == null)
            {
                throw new Exception($"Department not found: {departmentName}");
            }
            return department;
        }

        public async Task<SchoolYear> GetSchoolYearByNameAsync(string schoolYearName)
        {
            var schoolYear = await _repository.GetSchoolYearByNameAsync(schoolYearName);
            if (schoolYear == null)
            {
                throw new Exception($"SchoolYear not found: {schoolYearName}");
            }
            return schoolYear;
        }

        public async Task<StudyProgram> GetStudyProgramByNameAsync(string studyProgramName)
        {
            var studyProgram = await _repository.GetStudyProgramByNameAsync(studyProgramName);
            if (studyProgram == null)
            {
                throw new Exception($"StudyProgram not found: {studyProgramName}");
            }
            return studyProgram;
        }

        public async Task<StudentStatus> GetStudentStatusByNameAsync(string statusName)
        {
            var status = await _repository.GetStudentStatusByNameAsync(statusName);
            if (status == null)
            {
                throw new Exception($"Status not found: {statusName}");
            }
            return status;
        }

        public async Task<Identification> FindOrCreateIdentificationAsync(Identification identification)
        {
            return await _repository.FindOrCreateIdentificationAsync(identification);
        }

        public async Task<List<Student>> FilterDuplicateStudentsAsync(List<Student> students)
        {
            var existingIds = await _repository.GetExistingStudentIdsAsync();
            return students.Where(s => !existingIds.Contains(s.MSSV)).ToList();
        }

        public async Task AddStudentsAsync(List<Student> students)
        {
            await _repository.AddStudentsAsync(students);
        }

        public async Task ImportStudentsAsync(List<Student> students)
        {
            await _repository.ExecuteInTransactionAsync(async () =>
            {
                await _repository.AddStudentsAsync(students);
            });
        }

        public async Task<(bool isSuccess, string message, int importedCount)> ImportStudentsFromJsonAsync(IFormFile file, ILogger logger)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "File không hợp lệ.", 0);
            }

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var records = JsonConvert.DeserializeObject<List<StudentDto>>(json);
                if (records == null || !records.Any())
                {
                    return (false, "Dữ liệu JSON không hợp lệ.", 0);
                }

                var newStudents = new List<Student>();

                foreach (var record in records)
                {
                    var department = await GetDepartmentByNameAsync(record.Department);
                    var schoolYear = await GetSchoolYearByNameAsync(record.SchoolYear);
                    var studyProgram = await GetStudyProgramByNameAsync(record.StudyProgram);
                    var status = await GetStudentStatusByNameAsync(record.Status);

                    var diaChiNhanThu = await FindOrCreateAddressAsync(
                        record.AddressNhanThu_HouseNumber,
                        record.AddressNhanThu_StreetName,
                        record.AddressNhanThu_Ward,
                        record.AddressNhanThu_District,
                        record.AddressNhanThu_Province,
                        record.AddressNhanThu_Country);

                    var diaChiThuongTru = await FindOrCreateAddressAsync(
                        record.AddressThuongTru_HouseNumber,
                        record.AddressThuongTru_StreetName,
                        record.AddressThuongTru_Ward,
                        record.AddressThuongTru_District,
                        record.AddressThuongTru_Province,
                        record.AddressThuongTru_Country);

                    var diaChiTamTru = await FindOrCreateAddressAsync(
                        record.AddressTamTru_HouseNumber,
                        record.AddressTamTru_StreetName,
                        record.AddressTamTru_Ward,
                        record.AddressTamTru_District,
                        record.AddressTamTru_Province,
                        record.AddressTamTru_Country);

                    var identification = await FindOrCreateIdentificationAsync(new Identification
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

                var uniqueStudents = await FilterDuplicateStudentsAsync(newStudents);

                if (!uniqueStudents.Any())
                {
                    return (false, "Tất cả dữ liệu đã tồn tại trong hệ thống!", 0);
                }

                await ImportStudentsAsync(uniqueStudents);
                logger.LogInformation("JSON import successful.");

                return (true, "Import JSON thành công", uniqueStudents.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error importing JSON.");
                return (false, $"Lỗi khi import JSON: {ex.Message}", 0);
            }
        }

        public async Task<(bool isSuccess, string message, int importedCount)> ImportStudentsFromCsvAsync(IFormFile file, ILogger logger)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "File không hợp lệ.", 0);
            }

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

                csv.Context.RegisterClassMap<StudentMap>();
                var records = csv.GetRecords<StudentDto>().ToList();

                if (!records.Any())
                {
                    return (false, "Dữ liệu CSV không hợp lệ.", 0);
                }

                var newStudents = new List<Student>();

                foreach (var record in records)
                {
                    var department = await GetDepartmentByNameAsync(record.Department);
                    var schoolYear = await GetSchoolYearByNameAsync(record.SchoolYear);
                    var studyProgram = await GetStudyProgramByNameAsync(record.StudyProgram);
                    var status = await GetStudentStatusByNameAsync(record.Status);

                    var diaChiNhanThu = await FindOrCreateAddressAsync(
                        record.AddressNhanThu_HouseNumber,
                        record.AddressNhanThu_StreetName,
                        record.AddressNhanThu_Ward,
                        record.AddressNhanThu_District,
                        record.AddressNhanThu_Province,
                        record.AddressNhanThu_Country);

                    var diaChiThuongTru = await FindOrCreateAddressAsync(
                        record.AddressThuongTru_HouseNumber,
                        record.AddressThuongTru_StreetName,
                        record.AddressThuongTru_Ward,
                        record.AddressThuongTru_District,
                        record.AddressThuongTru_Province,
                        record.AddressThuongTru_Country);

                    var diaChiTamTru = await FindOrCreateAddressAsync(
                        record.AddressTamTru_HouseNumber,
                        record.AddressTamTru_StreetName,
                        record.AddressTamTru_Ward,
                        record.AddressTamTru_District,
                        record.AddressTamTru_Province,
                        record.AddressTamTru_Country);

                    var identification = await FindOrCreateIdentificationAsync(new Identification
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

                var uniqueStudents = await FilterDuplicateStudentsAsync(newStudents);

                if (!uniqueStudents.Any())
                {
                    return (false, "Tất cả dữ liệu đã tồn tại trong hệ thống!", 0);
                }

                await ImportStudentsAsync(uniqueStudents);
                logger.LogInformation("CSV import successful.");

                return (true, "Import CSV thành công.", uniqueStudents.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error importing CSV.");
                return (false, $"Lỗi khi import CSV: {ex.Message}", 0);
            }
        }
    }
}