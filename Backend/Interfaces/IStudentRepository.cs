using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudents(int page, int pageSize);
        Task<Student?> GetStudentById(string id);
        Task<int> GetStudentsCount();
        Task<bool> CreateStudent(Student student);
        Task<bool> UpdateStudent(Student student);
        Task<bool> DeleteStudent(string id);
        Task<bool> StudentExistsByPhoneNumber(string phoneNumber, string? StudentId = null);
        Task<bool> StudentExistsByEmail(string email, string? StudentId = null);
        Task<IEnumerable<Student>> SearchStudents(string keyword, int page, int pageSize);
        Task<Student?> GetStudentByIdAsync(string StudentId);
        Task<Student?> GetStudentByEmailAsync(string email);
    }
} 