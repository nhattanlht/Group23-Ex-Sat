using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services
{
    public class IdentificationService : IIdentificationService
    {
        private readonly IIdentificationRepository _repository;

        public IdentificationService(IIdentificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Identification> CreateIdentificationAsync(Identification identification)
        {
            return await _repository.AddAsync(identification);
        }

        public async Task<Identification?> GetIdentificationByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}