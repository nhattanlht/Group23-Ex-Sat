using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface ISchoolYearRepository
    {
        Task<List<SchoolYear>> GetAllAsync();
        Task<SchoolYear?> GetByIdAsync(int id);
        Task<SchoolYear> AddAsync(SchoolYear schoolYear);
        Task<bool> UpdateAsync(SchoolYear updated);
        Task<bool> DeleteAsync(int id);
    }
} 