using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;

        public EnrollmentService(IEnrollmentRepository repository)
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

        public async Task<bool> CancelEnrollmentAsync(int enrollmentId, string reason)
        {
            var enrollment = await _repository.GetByIdAsync(enrollmentId);
            if (enrollment == null || enrollment.IsCancelled) return false;

            var cancelDeadline = enrollment.Class.CancelDeadline;
            if (DateTime.Now > cancelDeadline) return false;

            enrollment.IsCancelled = true;
            enrollment.CancelReason = reason;
            enrollment.CancelDate = DateTime.Now;

            await _repository.UpdateAsync(enrollment);
            return true;
        }
    }
}
