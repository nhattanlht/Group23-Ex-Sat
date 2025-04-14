using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class SchoolYearService
    {
        private readonly SchoolYearRepository _repository;

        public SchoolYearService(SchoolYearRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SchoolYear>> GetAllSchoolYearsAsync() => _repository.GetAllAsync();

        public Task<SchoolYear?> GetSchoolYearByIdAsync(int id) => _repository.GetByIdAsync(id);

        public async Task<SchoolYear?> CreateSchoolYearAsync(SchoolYear schoolYear)
        {
            if (string.IsNullOrWhiteSpace(schoolYear.Name))
                return null;

            return await _repository.AddAsync(schoolYear);
        }

        public async Task<bool> UpdateSchoolYearAsync(int id, SchoolYear updatedSchoolYear)
        {
            if (id != updatedSchoolYear.Id) return false;
            return await _repository.UpdateAsync(updatedSchoolYear);
        }

        public Task<bool> DeleteSchoolYearAsync(int id) => _repository.DeleteAsync(id);
    }
}