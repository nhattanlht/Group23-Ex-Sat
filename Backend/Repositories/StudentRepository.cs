using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

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
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 10;

            return await _context
                .Students.Include(s => s.Department)
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
            return await _context
                .Students.Include(s => s.Department)
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
            var existingStudent = await _context
                .Students.Include(s => s.Department)
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
            if (student.DepartmentId != 0)
                existingStudent.DepartmentId = student.DepartmentId;
            if (student.SchoolYearId != 0)
                existingStudent.SchoolYearId = student.SchoolYearId;
            if (student.StudyProgramId != 0)
                existingStudent.StudyProgramId = student.StudyProgramId;
            if (student.StatusId != 0)
                existingStudent.StatusId = student.StatusId;

            if (existingStudent.Identification != null && student.Identification != null)
            {
                existingStudent.Identification.Number = student.Identification.Number;
                existingStudent.Identification.IssueDate = student.Identification.IssueDate;
                existingStudent.Identification.IssuedBy = student.Identification.IssuedBy;
                existingStudent.Identification.ExpiryDate = student.Identification.ExpiryDate;
                existingStudent.Identification.IdentificationType = student.Identification.IdentificationType;
                existingStudent.Identification.IssuingCountry = student.Identification.IssuingCountry;
                existingStudent.Identification.HasChip = student.Identification.HasChip;
                existingStudent.Identification.Notes = student.Identification.Notes;
            }

            // Update PermanentAddress
            if (existingStudent.PermanentAddress != null && student.PermanentAddress != null)
            {
                existingStudent.PermanentAddress.HouseNumber = student.PermanentAddress.HouseNumber;
                existingStudent.PermanentAddress.StreetName = student.PermanentAddress.StreetName;
                existingStudent.PermanentAddress.Ward = student.PermanentAddress.Ward;
                existingStudent.PermanentAddress.District = student.PermanentAddress.District;
                existingStudent.PermanentAddress.Province = student.PermanentAddress.Province;
                existingStudent.PermanentAddress.Country = student.PermanentAddress.Country;
            }
            if( existingStudent.RegisteredAddress != null && student.RegisteredAddress != null)
            {
                existingStudent.RegisteredAddress.HouseNumber = student.RegisteredAddress.HouseNumber;
                existingStudent.RegisteredAddress.StreetName = student.RegisteredAddress.StreetName;
                existingStudent.RegisteredAddress.Ward = student.RegisteredAddress.Ward;
                existingStudent.RegisteredAddress.District = student.RegisteredAddress.District;
                existingStudent.RegisteredAddress.Province = student.RegisteredAddress.Province;
                existingStudent.RegisteredAddress.Country = student.RegisteredAddress.Country;
            }
            if (existingStudent.TemporaryAddress != null && student.TemporaryAddress != null)
            {
                existingStudent.TemporaryAddress.HouseNumber = student.TemporaryAddress.HouseNumber;
                existingStudent.TemporaryAddress.StreetName = student.TemporaryAddress.StreetName;
                existingStudent.TemporaryAddress.Ward = student.TemporaryAddress.Ward;
                existingStudent.TemporaryAddress.District = student.TemporaryAddress.District;
                existingStudent.TemporaryAddress.Province = student.TemporaryAddress.Province;
                existingStudent.TemporaryAddress.Country = student.TemporaryAddress.Country;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StudentExistsByPhoneNumber(
            string phoneNumber,
            string? StudentId = null
        )
        {
            return await _context.Students.AnyAsync(s =>
                s.PhoneNumber == phoneNumber && (StudentId == null || s.StudentId != StudentId)
            );
        }

        public async Task<bool> StudentExistsByEmail(string email, string? StudentId = null)
        {
            return await _context.Students.AnyAsync(s =>
                s.Email == email && (StudentId == null || s.StudentId != StudentId)
            );
        }

        public async Task<IEnumerable<Student>> SearchStudents(
            string keyword,
            int page,
            int pageSize
        )
        {
            var query = _context
                .Students.Include(s => s.Department)
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
                query = query.Where(s =>
                    EF.Functions.Collate(s.FullName, "Latin1_General_CI_AI").Contains(keyword)
                    || s.StudentId.Contains(keyword)
                );
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
