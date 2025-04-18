using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _repository;

        public GradeService(IGradeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Grade>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Grade?> GetByIdAsync(string MSSV, string classId) => _repository.GetByIdAsync(MSSV, classId);
        public Task AddAsync(Grade grade) => _repository.AddAsync(grade);
        public Task UpdateAsync(Grade grade) => _repository.UpdateAsync(grade);
        public Task DeleteAsync(int gradeId) => _repository.DeleteAsync(gradeId);
    }

}
