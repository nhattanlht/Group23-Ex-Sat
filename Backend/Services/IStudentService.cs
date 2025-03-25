using StudentManagement.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface IStudentService
    {
        Task<(IEnumerable<Student>, int, int)> GetStudents(int page, int pageSize);
        Task<Student> GetStudentById(string id);
        Task<(bool Success, string Message)> CreateStudent(Student student);
        Task<(bool Success, string Message)> UpdateStudent(Student student);
        Task<bool> DeleteStudent(string id);
        
        Task<(IEnumerable<Student>, int, int)> SearchStudents(StudentFilterModel filter, int page, int pageSize);
    }
}