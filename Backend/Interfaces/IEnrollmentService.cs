using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IEnrollmentService
    {
        Task<List<Enrollment>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int id);
        Task<bool> HasPrerequisiteAsync(string studentId, string classId);
        Task<bool> IsClassFullAsync(string classId);
        Task AddAsync(Enrollment enrollment);
        Task UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(int id);
        Task<bool> CancelEnrollmentAsync(int enrollmentId, string reason);
    }
} 