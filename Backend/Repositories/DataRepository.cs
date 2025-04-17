using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.DTOs;

namespace StudentManagement.Repositories
{
    public class DataRepository
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
                .Include(s => s.DiaChiNhanThu)
                .Include(s => s.DiaChiThuongTru)
                .Include(s => s.DiaChiTamTru)
                .Include(s => s.StudentStatus)
                .Select(s => new StudentDto
                {
                    MSSV = s.MSSV,
                    HoTen = s.HoTen,
                    NgaySinh = s.NgaySinh,
                    GioiTinh = s.GioiTinh,
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
                    Email = s.Email,
                    SoDienThoai = s.SoDienThoai,
                    QuocTich = s.QuocTich,
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
            return await _context.Students.Select(s => s.MSSV).ToHashSetAsync();
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

        public async Task<BinaryData?> GetDataByIdAsync(int id)
        {
            var data = await _context.Data.FindAsync(id);
            return data != null ? new BinaryData(data) : null; // Convert to BinaryData
        }
    }
}