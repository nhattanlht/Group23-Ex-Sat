using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<(bool exists, string message)> CheckDuplicateAsync(string name);
        Task<Department> CreateAsync(Department department);
        Task<bool> UpdateAsync(int id, Department department);
        Task<bool> DeleteAsync(int id);
    }
} 