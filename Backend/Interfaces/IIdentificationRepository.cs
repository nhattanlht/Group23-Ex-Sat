using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IIdentificationRepository
    {
        Task<Identification?> GetByIdAsync(int id);
        Task<Identification> AddAsync(Identification identification);
    }
} 