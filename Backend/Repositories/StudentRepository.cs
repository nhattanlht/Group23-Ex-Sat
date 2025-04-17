using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public class StudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetStudents(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            return await _context.Students
                .OrderBy(s => s.MSSV)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentById(string id)
        {
            return await _context.Students
                                 .Include(s => s.Department)
                                 .Include(s => s.SchoolYear)
                                 .Include(s => s.StudyProgram)
                                 .Include(s => s.StudentStatus)
                                 .Include(s => s.DiaChiNhanThu)
                                 .Include(s => s.DiaChiThuongTru)
                                 .Include(s => s.DiaChiTamTru)
                                 .FirstOrDefaultAsync(s => s.MSSV == id);
        }

        public async Task<int> GetStudentsCount()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<bool> CreateStudent(Student student)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStudent(Student student)
        {
            _context.Update(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StudentExistsByPhoneNumber(string phoneNumber, string studentId = null)
        {
            return await _context.Students
                .AnyAsync(s => s.SoDienThoai == phoneNumber && (studentId == null || s.MSSV != studentId));
        }

        public async Task<bool> StudentExistsByEmail(string email, string studentId = null)
        {
            return await _context.Students
                .AnyAsync(s => s.Email == email && (studentId == null || s.MSSV != studentId));
        }

        public async Task<IEnumerable<Student>> SearchStudents(string keyword, int page, int pageSize)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => EF.Functions.Collate(
                        s.HoTen, "Latin1_General_CI_AI").Contains(keyword) ||
                        s.MSSV.Contains(keyword));
            }

            return await query
                .OrderBy(s => s.MSSV)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.MSSV == studentId);
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}