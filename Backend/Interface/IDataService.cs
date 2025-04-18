using StudentManagement.Models;
using StudentManagement.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace StudentManagement.Services
{
    public interface IDataService
    {
        Task<List<StudentDto>> GetAllStudentsAsync();
        Task<Address> FindOrCreateAddressAsync(string? houseNumber, string? streetName, string? ward, string? district, string? province, string? country);
        Task<Department> GetDepartmentByNameAsync(string departmentName);
        Task<SchoolYear> GetSchoolYearByNameAsync(string schoolYearName);
        Task<StudyProgram> GetStudyProgramByNameAsync(string studyProgramName);
        Task<StudentStatus> GetStudentStatusByNameAsync(string statusName);
        Task<Identification> FindOrCreateIdentificationAsync(Identification identification);
        Task<List<Student>> FilterDuplicateStudentsAsync(List<Student> students);
        Task AddStudentsAsync(List<Student> students);
        Task ImportStudentsAsync(List<Student> students);
        Task<(bool isSuccess, string message, int importedCount)> ImportStudentsFromJsonAsync(IFormFile file, ILogger logger);
        Task<(bool isSuccess, string message, int importedCount)> ImportStudentsFromCsvAsync(IFormFile file, ILogger logger);
    }
}