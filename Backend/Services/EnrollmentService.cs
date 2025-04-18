using StudentManagement.Models;
using StudentManagement.Repositories;

public class EnrollmentService
{
    private readonly EnrollmentRepository _repository;

    public EnrollmentService(EnrollmentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Enrollment>> GetAllAsync() => _repository.GetAllAsync();
    public Task<Enrollment?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    public Task<bool> HasPrerequisiteAsync(string studentId, string classId) => _repository.HasPrerequisiteAsync(studentId, classId);
    public Task<bool> IsClassFullAsync(string classId) => _repository.IsClassFullAsync(classId);
    public Task AddAsync(Enrollment enrollment) => _repository.AddAsync(enrollment);
    public Task UpdateAsync(Enrollment enrollment) => _repository.UpdateAsync(enrollment);
    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
}
