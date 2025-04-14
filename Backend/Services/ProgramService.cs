using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class ProgramService
    {
        private readonly ProgramRepository _repository;

        public ProgramService(ProgramRepository repository)
        {
            _repository = repository;
        }

        public Task<List<StudyProgram>> GetAllProgramsAsync() => _repository.GetAllAsync();

        public Task<StudyProgram?> GetProgramByIdAsync(int id) => _repository.GetByIdAsync(id);

        public async Task<StudyProgram?> CreateProgramAsync(StudyProgram program)
        {
            if (await _repository.ExistsByNameAsync(program.Name))
                return null;

            return await _repository.AddAsync(program);
        }

        public async Task<bool> UpdateProgramAsync(int id, StudyProgram updatedProgram)
        {
            if (id != updatedProgram.Id) return false;
            return await _repository.UpdateAsync(updatedProgram);
        }

        public Task<bool> DeleteProgramAsync(int id) => _repository.DeleteAsync(id);
    }
}