using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.DTOs;

namespace StudentManagement.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly ApplicationDbContext _context;

        public DataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.Department)
                .Include(s => s.SchoolYear)
                .Include(s => s.StudyProgram)
                .Include(s => s.PermanentAddress)
                .Include(s => s.RegisteredAddress)
                .Include(s => s.TemporaryAddress)
                .Include(s => s.StudentStatus)
                .Select(s => new StudentDto
                {
                    StudentId = s.StudentId,
                    FullName = s.FullName,
                    DateOfBirth = s.DateOfBirth,
                    Gender = s.Gender,
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
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Nationality = s.Nationality,
                    Identification_Type = s.Identification.IdentificationType,
                    Identification_Number = s.Identification.Number,
                    Identification_IssueDate = s.Identification.IssueDate,
                    Identification_ExpiryDate = s.Identification.ExpiryDate,
                    Identification_IssuedBy = s.Identification.IssuedBy,
                    Identification_HasChip = s.Identification.HasChip ?? false,
                    Identification_IssuingCountry = s.Identification.IssuingCountry,
                    Identification_Notes = s.Identification.Notes,
                    Status = s.StudentStatus.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Address> FindOrCreateAddressAsync(Address address)
        {
            var existingAddress = await _context.Addresses.FirstOrDefaultAsync(a =>
                a.HouseNumber == address.HouseNumber &&
                a.StreetName == address.StreetName &&
                a.Ward == address.Ward &&
                a.District == address.District &&
                a.Province == address.Province &&
                a.Country == address.Country);

            if (existingAddress != null)
            {
                return existingAddress;
            }

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Department?> GetDepartmentByNameAsync(string departmentName)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Name == departmentName);
        }

        public async Task<SchoolYear?> GetSchoolYearByNameAsync(string schoolYearName)
        {
            return await _context.SchoolYears.FirstOrDefaultAsync(sy => sy.Name == schoolYearName);
        }

        public async Task<StudyProgram?> GetStudyProgramByNameAsync(string studyProgramName)
        {
            return await _context.StudyPrograms.FirstOrDefaultAsync(sp => sp.Name == studyProgramName);
        }

        public async Task<StudentStatus?> GetStudentStatusByNameAsync(string statusName)
        {
            return await _context.StudentStatuses.FirstOrDefaultAsync(st => st.Name == statusName);
        }

        public async Task<Identification> FindOrCreateIdentificationAsync(Identification identification)
        {
            var existingIdentification = await _context.Identifications.FirstOrDefaultAsync(i => i.Number == identification.Number);

            if (existingIdentification != null)
            {
                return existingIdentification;
            }

            _context.Identifications.Add(identification);
            await _context.SaveChangesAsync();
            return identification;
        }

        public async Task<HashSet<string>> GetExistingStudentIdsAsync()
        {
            return await _context.Students.Select(s => s.StudentId).ToHashSetAsync();
        }

        public async Task AddStudentsAsync(List<Student> students)
        {
            await _context.Students.AddRangeAsync(students);
            await _context.SaveChangesAsync();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}