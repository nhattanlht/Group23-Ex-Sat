using StudentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IProgramService
    {
        Task<List<StudyProgram>> GetAllProgramsAsync();
        Task<StudyProgram?> GetProgramByIdAsync(int id);
        Task<StudyProgram?> CreateProgramAsync(StudyProgram program);
        Task<bool> UpdateProgramAsync(int id, StudyProgram updatedProgram);
        Task<bool> DeleteProgramAsync(int id);
    }
} 