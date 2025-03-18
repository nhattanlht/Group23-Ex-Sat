using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Student>, int, int)> GetStudents(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10; // Default to 10 students per page

            var totalStudents = await _context.Students.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            var students = await _context.Students
                .OrderBy(s => s.MSSV) // Sorting by student ID (change if needed)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalStudents, totalPages);
        }

        public async Task<Student> GetStudentById(string id)
        {
            return await _context.Students
                                 .Include(s => s.Department)
                                 .Include(s => s.SchoolYear)
                                 .Include(s => s.StudyProgram)
                                 .Include(s => s.StudentStatus)
                                 .FirstOrDefaultAsync(s => s.MSSV == id);
        }

        public async Task<(bool Success, string Message)> CreateStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.SoDienThoai))
                return (false, "Số điện thoại không được để trống.");

            if (!Regex.IsMatch(student.SoDienThoai, @"^(0[2-9]|84[2-9])\d{8,9}$"))
                return (false, "Số điện thoại không hợp lệ.");

            if (await _context.Students.AnyAsync(s => s.SoDienThoai == student.SoDienThoai))
                return (false, "Số điện thoại đã tồn tại trong hệ thống.");

            if (string.IsNullOrWhiteSpace(student.Email))
                return (false, "Email không được để trống.");

            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Email không hợp lệ.");

            if (await _context.Students.AnyAsync(s => s.Email == student.Email))
                return (false, "Email đã tồn tại trong hệ thống.");

            try
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return (true, "Sinh viên được tạo thành công.");
            }
            catch
            {
                return (false, "Đã xảy ra lỗi khi tạo sinh viên.");
            }
        }


        public async Task<(bool Success, string Message)> UpdateStudent(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.MSSV);
            if (existingStudent == null) return (false, "Sinh viên không tồn tại.");

            if (string.IsNullOrWhiteSpace(student.SoDienThoai))
                return (false, "Số điện thoại không được để trống.");

            if (!Regex.IsMatch(student.SoDienThoai, @"^(0[2-9]|84[2-9])\d{8,9}$"))
                return (false, "Số điện thoại không hợp lệ.");

            if (await _context.Students.AnyAsync(s => s.SoDienThoai == student.SoDienThoai && s.MSSV != student.MSSV))
                return (false, "Số điện thoại đã tồn tại trong hệ thống.");

            if (string.IsNullOrWhiteSpace(student.Email))
                return (false, "Email không được để trống.");

            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Email không hợp lệ.");

            if (await _context.Students.AnyAsync(s => s.Email == student.Email && s.MSSV != student.MSSV))
                return (false, "Email đã tồn tại trong hệ thống.");

            try
            {
                existingStudent.HoTen = student.HoTen;
                existingStudent.NgaySinh = student.NgaySinh;
                existingStudent.GioiTinh = student.GioiTinh;
                existingStudent.DepartmentId = student.DepartmentId;
                existingStudent.StatusId = student.StatusId;
                existingStudent.SchoolYearId = student.SchoolYearId;
                existingStudent.StudyProgramId = student.StudyProgramId;
                existingStudent.DiaChi = student.DiaChi;
                existingStudent.Email = student.Email;
                existingStudent.SoDienThoai = student.SoDienThoai;

                _context.Update(existingStudent);
                await _context.SaveChangesAsync();
                return (true, "Cập nhật thông tin sinh viên thành công.");
            }
            catch
            {
                return (false, "Đã xảy ra lỗi khi cập nhật sinh viên.");
            }
        }


        public async Task<bool> DeleteStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Student>> SearchStudents(string keyword, int page, int pageSize)
        {
            return await _context.Students
                .Where(s => s.HoTen.Contains(keyword) || s.MSSV.Contains(keyword))
                .OrderBy(s => s.MSSV)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}