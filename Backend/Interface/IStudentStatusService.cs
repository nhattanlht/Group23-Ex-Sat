using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IStudentStatusService
    {
        Task<List<StudentStatus>> GetAllStudentStatusesAsync();
        Task<StudentStatus?> GetStudentStatusByIdAsync(int id);
        Task<StudentStatus?> CreateStudentStatusAsync(StudentStatus studentStatus);
        Task<bool> UpdateStudentStatusAsync(int id, StudentStatus studentStatus);
        Task<bool> DeleteStudentStatusAsync(int id);
    }
} 