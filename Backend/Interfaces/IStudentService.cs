using StudentManagement.Models;
using StudentManagement.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IStudentService
    {
        Task<(IEnumerable<Student>, int, int)> GetStudents(int page, int pageSize);
        Task<Student> GetStudentById(string id);
        Task<(bool Success, string Message)> CreateStudent(Student student);
        Task<(bool Success, string Message)> UpdateStudent(Student student);
        Task<bool> DeleteStudent(string id);
        Task<(IEnumerable<Student>, int, int)> SearchStudents(StudentFilterModel filters, int page, int pageSize);
    }
} 