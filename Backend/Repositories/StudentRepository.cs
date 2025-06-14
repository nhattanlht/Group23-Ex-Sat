using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public class StudentRepository : IStudentRepository
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
                .Include(s => s.Department)
                .Include(s => s.SchoolYear)
                .Include(s => s.StudyProgram)
                .Include(s => s.StudentStatus)
                .Include(s => s.PermanentAddress)
                .Include(s => s.RegisteredAddress)
                .Include(s => s.TemporaryAddress)
                .Include(s => s.Identification)
                .OrderBy(s => s.StudentId)
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
                                 .Include(s => s.PermanentAddress)
                                 .Include(s => s.RegisteredAddress)
                                 .Include(s => s.TemporaryAddress)
                                 .Include(s => s.Identification)
                                 .FirstOrDefaultAsync(s => s.StudentId == id);
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
            var existingStudent = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.SchoolYear)
                .Include(s => s.StudyProgram)
                .Include(s => s.StudentStatus)
                .Include(s => s.PermanentAddress)
                .Include(s => s.RegisteredAddress)
                .Include(s => s.TemporaryAddress)
                .Include(s => s.Identification)
                .FirstOrDefaultAsync(s => s.StudentId == student.StudentId);

            if (existingStudent == null)
                return false;

            // Update basic properties
            existingStudent.FullName = student.FullName;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Gender = student.Gender;
            existingStudent.Email = student.Email;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.Nationality = student.Nationality;

            // Update related entities if they are provided
            if (student.Department != null)
                existingStudent.Department = student.Department;
            if (student.SchoolYear != null)
                existingStudent.SchoolYear = student.SchoolYear;
            if (student.StudyProgram != null)
                existingStudent.StudyProgram = student.StudyProgram;
            if (student.StudentStatus != null)
                existingStudent.StudentStatus = student.StudentStatus;

            // Update addresses if they are provided
            if (student.PermanentAddress != null)
                existingStudent.PermanentAddress = student.PermanentAddress;
            if (student.RegisteredAddress != null)
                existingStudent.RegisteredAddress = student.RegisteredAddress;
            if (student.TemporaryAddress != null)
                existingStudent.TemporaryAddress = student.TemporaryAddress;

            // Update identification if provided
            if (student.Identification != null)
                existingStudent.Identification = student.Identification;

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

        public async Task<bool> StudentExistsByPhoneNumber(string phoneNumber, string? StudentId = null)
        {
            return await _context.Students
                .AnyAsync(s => s.PhoneNumber == phoneNumber && (StudentId == null || s.StudentId != StudentId));
        }

        public async Task<bool> StudentExistsByEmail(string email, string? StudentId = null)
        {
            return await _context.Students
                .AnyAsync(s => s.Email == email && (StudentId == null || s.StudentId != StudentId));
        }

        public async Task<IEnumerable<Student>> SearchStudents(string keyword, int page, int pageSize)
        {
            var query = _context.Students
                .Include(s => s.Department)
                .Include(s => s.SchoolYear)
                .Include(s => s.StudyProgram)
                .Include(s => s.StudentStatus)
                .Include(s => s.PermanentAddress)
                .Include(s => s.RegisteredAddress)
                .Include(s => s.TemporaryAddress)
                .Include(s => s.Identification)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => EF.Functions.Collate(
                        s.FullName, "Latin1_General_CI_AI").Contains(keyword) ||
                        s.StudentId.Contains(keyword));
            }

            return await query
                .OrderBy(s => s.StudentId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(string StudentId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == StudentId);
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}