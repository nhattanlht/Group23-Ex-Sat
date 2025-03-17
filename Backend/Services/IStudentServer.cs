using StudentManagement.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StudentManagement.Services
{
    public interface IStudentService
    {
        Task<(IEnumerable<Student>, int, int)> GetStudents(int page, int pageSize);
        Task<Student> GetStudentById(string id);
        Task<bool> CreateStudent(Student student);
        Task<bool> UpdateStudent(Student student);
        Task<bool> DeleteStudent(string id);
        Task<IEnumerable<Student>> SearchStudents(string keyword, int page, int pageSize);
    }
}