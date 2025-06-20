using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IGradeService
    {
        public Task<List<Grade>> GetAllAsync();
        public Task<Grade?> GetByIdAsync(string StudentId, string classId);
        public Task AddAsync(Grade grade);
        public Task UpdateAsync(Grade grade);
        public Task DeleteAsync(int gradeId);
    }

}
