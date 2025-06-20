using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int enrollmentId);
        Task<bool> HasPrerequisiteAsync(string StudentId, string classId);
        Task<bool> IsClassFullAsync(string classId);
        Task AddAsync(Enrollment enrollment);
        Task UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(int enrollmentId);
        Task<bool> HasEnrollmentForClassAsync(string classId);
    }
} 