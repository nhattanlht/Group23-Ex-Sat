using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        void Add(Department department);
        void Remove(Department department);
        Task SaveChangesAsync();
    }
} 