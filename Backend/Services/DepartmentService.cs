using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class DepartmentService
    {
        private readonly DepartmentRepository _repository;

        public DepartmentService(DepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<(bool exists, string message)> CheckDuplicateAsync(string name)
        {
            var exists = await _repository.ExistsByNameAsync(name);
            return (exists, exists ? "Khoa đã tồn tại!" : string.Empty);
        }

        public async Task<Department> CreateAsync(Department department)
        {
            _repository.Add(department);
            await _repository.SaveChangesAsync();
            return department;
        }

        public async Task<bool> UpdateAsync(int id, Department department)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = department.Name;
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department == null) return false;

            _repository.Remove(department);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}