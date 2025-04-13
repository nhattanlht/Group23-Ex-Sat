using StudentManagement.Models;
using StudentManagement.DTOs;
using StudentManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Services
{
    public class DataService
    {
        private readonly DataRepository _repository;

        public DataService(DataRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            return await _repository.GetAllStudentsAsync();
        }

        public async Task<Address> FindOrCreateAddressAsync(string houseNumber, string streetName, string ward, string district, string province, string country)
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
    }
}