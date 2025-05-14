using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IStudentStatusRepository
    {
        Task<List<StudentStatus>> GetAllAsync();
        Task<StudentStatus?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task<StudentStatus> AddAsync(StudentStatus studentStatus);
        Task<bool> UpdateAsync(StudentStatus updated);
        Task<bool> DeleteAsync(int id);
    }
} 