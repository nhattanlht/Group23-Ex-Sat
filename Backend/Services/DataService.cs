using StudentManagement.Models;
using StudentManagement.Repositories;
using Newtonsoft.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Localization;

namespace StudentManagement.Services
{
    public class DataService: IDataService
    {
        private readonly IDataRepository _repository;
        private readonly IStringLocalizer<SharedResource> _localizer;


        public DataService(IDataRepository repository, IStringLocalizer<SharedResource> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            return await _repository.GetAllStudentsAsync();
        }

        public async Task<Address> FindOrCreateAddressAsync(string? houseNumber, string? streetName, string? ward, string? district, string? province, string? country)
        {
            
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
                throw new Exception(
                    _localizer["InvalidAddressData"].Value);
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
                throw new Exception(_localizer["DepartmentNotFound", departmentName].Value);
            }
            return department;
        }

        public async Task<SchoolYear> GetSchoolYearByNameAsync(string schoolYearName)
        {
            var schoolYear = await _repository.GetSchoolYearByNameAsync(schoolYearName);
            if (schoolYear == null)
            {
                throw new Exception(_localizer["SchoolYearNotFound", schoolYearName].Value);
            }
            return schoolYear;
        }

        public async Task<StudyProgram> GetStudyProgramByNameAsync(string studyProgramName)
        {
            var studyProgram = await _repository.GetStudyProgramByNameAsync(studyProgramName);
            if (studyProgram == null)
            {
                throw new Exception(_localizer["StudyProgramNotFound", studyProgramName].Value);
            }
            return studyProgram;
        }

        public async Task<StudentStatus> GetStudentStatusByNameAsync(string statusName)
        {
            var status = await _repository.GetStudentStatusByNameAsync(statusName);
            if (status == null)
            {
                throw new Exception(_localizer["StatusNotFound", statusName].Value);
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
            return students.Where(s => !existingIds.Contains(s.StudentId)).ToList();
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
                return (false, _localizer["InvalidFile"].Value, 0);
            }

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var records = JsonConvert.DeserializeObject<List<StudentDto>>(json);
                if (records == null || !records.Any())
                {
                    return (false, _localizer["InvalidJsonData"], 0);
                }

                var newStudents = new List<Student>();

                foreach (var record in records)
                {
                    var department = await GetDepartmentByNameAsync(record.Department);
                    var schoolYear = await GetSchoolYearByNameAsync(record.SchoolYear);
                    var studyProgram = await GetStudyProgramByNameAsync(record.StudyProgram);
                    var status = await GetStudentStatusByNameAsync(record.Status);

                    var PermanentAddress = await FindOrCreateAddressAsync(
                        record.PermanentAddress_HouseNumber,
                        record.PermanentAddress_StreetName,
                        record.PermanentAddress_Ward,
                        record.PermanentAddress_District,
                        record.PermanentAddress_Province,
                        record.PermanentAddress_Country);

                    var RegisteredAddress = await FindOrCreateAddressAsync(
                        record.RegisteredAddress_HouseNumber,
                        record.RegisteredAddress_StreetName,
                        record.RegisteredAddress_Ward,
                        record.RegisteredAddress_District,
                        record.RegisteredAddress_Province,
                        record.RegisteredAddress_Country);

                    var TemporaryAddress = await FindOrCreateAddressAsync(
                        record.TemporaryAddress_HouseNumber,
                        record.TemporaryAddress_StreetName,
                        record.TemporaryAddress_Ward,
                        record.TemporaryAddress_District,
                        record.TemporaryAddress_Province,
                        record.TemporaryAddress_Country);

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
                        PermanentAddressId = PermanentAddress.Id,
                        RegisteredAddressId = RegisteredAddress?.Id ?? null,
                        TemporaryAddressId = TemporaryAddress?.Id ?? null,
                        IdentificationId = identification.Id
                    };

                    newStudents.Add(student);
                }

                var uniqueStudents = await FilterDuplicateStudentsAsync(newStudents);

                if (!uniqueStudents.Any())
                {
                    return (false, _localizer["AllDataExists"].Value, 0);
                }

                await ImportStudentsAsync(uniqueStudents);
                logger.LogInformation("JSON import successful.");

                return (true, _localizer["ImportJsonSuccess", uniqueStudents.Count], uniqueStudents.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error importing JSON.");
                return (false, _localizer["ImportJsonError", ex.Message].Value, 0);
            }
        }

        public async Task<(bool isSuccess, string message, int importedCount)> ImportStudentsFromCsvAsync(IFormFile file, ILogger logger)
        {
            if (file == null || file.Length == 0)
            {
                return (false, _localizer["InvalidFile"].Value, 0);
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
                    return (false, _localizer["InvalidCsvData"].Value, 0);
                }

                var newStudents = new List<Student>();

                foreach (var record in records)
                {
                    var department = await GetDepartmentByNameAsync(record.Department);
                    var schoolYear = await GetSchoolYearByNameAsync(record.SchoolYear);
                    var studyProgram = await GetStudyProgramByNameAsync(record.StudyProgram);
                    var status = await GetStudentStatusByNameAsync(record.Status);

                    var PermanentAddress = await FindOrCreateAddressAsync(
                        record.PermanentAddress_HouseNumber,
                        record.PermanentAddress_StreetName,
                        record.PermanentAddress_Ward,
                        record.PermanentAddress_District,
                        record.PermanentAddress_Province,
                        record.PermanentAddress_Country);

                    var RegisteredAddress = await FindOrCreateAddressAsync(
                        record.RegisteredAddress_HouseNumber,
                        record.RegisteredAddress_StreetName,
                        record.RegisteredAddress_Ward,
                        record.RegisteredAddress_District,
                        record.RegisteredAddress_Province,
                        record.RegisteredAddress_Country);

                    var TemporaryAddress = await FindOrCreateAddressAsync(
                        record.TemporaryAddress_HouseNumber,
                        record.TemporaryAddress_StreetName,
                        record.TemporaryAddress_Ward,
                        record.TemporaryAddress_District,
                        record.TemporaryAddress_Province,
                        record.TemporaryAddress_Country);

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
                        PermanentAddressId = PermanentAddress.Id,
                        RegisteredAddressId = RegisteredAddress?.Id ?? null,
                        TemporaryAddressId = TemporaryAddress?.Id ?? null,
                        IdentificationId = identification.Id
                    };

                    newStudents.Add(student);
                }

                var uniqueStudents = await FilterDuplicateStudentsAsync(newStudents);

                if (!uniqueStudents.Any())
                {
                    return (false, _localizer["AllDataExists"].Value, 0);
                }

                await ImportStudentsAsync(uniqueStudents);
                logger.LogInformation("CSV import successful.");

                return (true, _localizer["ImportCsvSuccess", uniqueStudents.Count].Value, uniqueStudents.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error importing CSV.");
                return (false, _localizer["ImportCsvError", ex.Message].Value, 0);
            }
        }
    }
}