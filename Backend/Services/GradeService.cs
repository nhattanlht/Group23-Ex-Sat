using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class GradeService
    {
        private readonly GradeRepository _repository;

        public GradeService(GradeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Grade>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Grade?> GetByIdAsync(string studentId, string classId) => _repository.GetByIdAsync(studentId, classId);
        public Task AddAsync(Grade grade) => _repository.AddAsync(grade);
        public Task UpdateAsync(Grade grade) => _repository.UpdateAsync(grade);
        public Task DeleteAsync(string studentId, string classId) => _repository.DeleteAsync(studentId, classId);
    }

}
