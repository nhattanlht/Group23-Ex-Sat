using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public interface IDataRepository
    {
        Task<List<StudentDto>> GetAllStudentsAsync();
        Task<Address> FindOrCreateAddressAsync(Address address);
        Task<Department?> GetDepartmentByNameAsync(string departmentName);
        Task<SchoolYear?> GetSchoolYearByNameAsync(string schoolYearName);
        Task<StudyProgram?> GetStudyProgramByNameAsync(string studyProgramName);
        Task<StudentStatus?> GetStudentStatusByNameAsync(string statusName);
        Task<Identification> FindOrCreateIdentificationAsync(Identification identification);
        Task<HashSet<string>> GetExistingStudentIdsAsync();
        Task AddStudentsAsync(List<Student> students);
        Task ExecuteInTransactionAsync(Func<Task> action);
        Task<BinaryData?> GetDataByIdAsync(int id);
    }
}