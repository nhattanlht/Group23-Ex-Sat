using Microsoft.Extensions.Localization;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ClassService(
            IClassRepository repository,
            IEnrollmentRepository enrollmentRepository,
            IGradeRepository gradeRepository,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _classRepository = repository;
            _enrollmentRepository = enrollmentRepository;
            _gradeRepository = gradeRepository;
            _localizer = localizer;
        }

        public Task<List<Class>> GetAllAsync() => _classRepository.GetAllAsync();

        public Task<Class?> GetByIdAsync(string classId) => _classRepository.GetByIdAsync(classId);

        public async Task AddAsync(Class classEntity)
        {
            var existingClass = await _classRepository.GetByIdAsync(classEntity.ClassId);
            if (existingClass != null)
                throw new InvalidOperationException(_localizer["ClassCodeExists"].Value);

            _classRepository.AddAsync(classEntity);
        }

        public Task UpdateAsync(Class classEntity) => _classRepository.UpdateAsync(classEntity);

        public async Task DeleteAsync(string classId)
        {
            bool hasEnrollments = await _enrollmentRepository.HasEnrollmentForClassAsync(classId);
            bool hasGrades = await _gradeRepository.HasGradeForClassAsync(classId);

            if (hasEnrollments || hasGrades)
            {
                throw new InvalidOperationException(_localizer["DeleteClassError"].Value);
            }

            await _classRepository.DeleteAsync(classId);
        }
    }
}
