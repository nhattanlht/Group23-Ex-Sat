using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(string classId);
        Task AddAsync(Class classEntity);
        Task UpdateAsync(Class classEntity);
        Task DeleteAsync(string classId);
    }
} 