using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IIdentificationService
    {
        Task<Identification> CreateIdentificationAsync(Identification identification);
        Task<Identification?> GetIdentificationByIdAsync(int id);
    }
} 