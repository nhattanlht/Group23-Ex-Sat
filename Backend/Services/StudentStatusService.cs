using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class StudentStatusService
    {
        private readonly StudentStatusRepository _repository;

        public StudentStatusService(StudentStatusRepository repository)
        {
            _repository = repository;
        }

        public Task<List<StudentStatus>> GetAllStudentStatusesAsync() => _repository.GetAllAsync();

        public Task<StudentStatus?> GetStudentStatusByIdAsync(int id) => _repository.GetByIdAsync(id);

        public async Task<StudentStatus?> CreateStudentStatusAsync(StudentStatus studentStatus)
        {
            if (await _repository.ExistsAsync(studentStatus.Name))
                return null;

            return await _repository.AddAsync(studentStatus);
        }

        public async Task<bool> UpdateStudentStatusAsync(int id, StudentStatus studentStatus)
        {
            if (id != studentStatus.Id) return false;
            return await _repository.UpdateAsync(studentStatus);
        }

        public Task<bool> DeleteStudentStatusAsync(int id) => _repository.DeleteAsync(id);
    }
}