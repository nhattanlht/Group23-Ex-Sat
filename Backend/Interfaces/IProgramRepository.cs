using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IProgramRepository
    {
        Task<List<StudyProgram>> GetAllAsync();
        Task<StudyProgram?> GetByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task<StudyProgram> AddAsync(StudyProgram program);
        Task<bool> UpdateAsync(StudyProgram updated);
        Task<bool> DeleteAsync(int id);
    }
} 