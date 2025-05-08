using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public class StudentStatusRepository : IStudentStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<StudentStatus>> GetAllAsync() =>
            _context.StudentStatuses.ToListAsync();

        public Task<StudentStatus?> GetByIdAsync(int id) =>
            _context.StudentStatuses.FindAsync(id).AsTask();

        public Task<bool> ExistsAsync(string name) =>
            _context.StudentStatuses.AnyAsync(s => s.Name == name);

        public async Task<StudentStatus> AddAsync(StudentStatus studentStatus)
        {
            _context.StudentStatuses.Add(studentStatus);
            await _context.SaveChangesAsync();
            return studentStatus;
        }

        public async Task<bool> UpdateAsync(StudentStatus updated)
        {
            var existingStatus = await _context.StudentStatuses.FindAsync(updated.Id);
            if (existingStatus == null) return false;

            existingStatus.Name = updated.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await _context.StudentStatuses.Include(s => s.Students).FirstOrDefaultAsync(s => s.Id == id);
            if (status == null || status.Students.Any()) return false;

            _context.StudentStatuses.Remove(status);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}