using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class ClassService
    {
        private readonly ClassRepository _repository;

        public ClassService(ClassRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Class>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Class?> GetByIdAsync(string classId) => _repository.GetByIdAsync(classId);
        public Task AddAsync(Class classEntity) => _repository.AddAsync(classEntity);
        public Task UpdateAsync(Class classEntity) => _repository.UpdateAsync(classEntity);
        public Task DeleteAsync(string classId) => _repository.DeleteAsync(classId);
    }
}