using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface ISchoolYearService
    {
        Task<List<SchoolYear>> GetAllSchoolYearsAsync();
        Task<SchoolYear?> GetSchoolYearByIdAsync(int id);
        Task<SchoolYear?> CreateSchoolYearAsync(SchoolYear schoolYear);
        Task<bool> UpdateSchoolYearAsync(int id, SchoolYear updatedSchoolYear);
        Task<bool> DeleteSchoolYearAsync(int id);
    }
} 