using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using StudentManagement.DTOs;

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
                .Select(s => new Student
                {
                    MSSV = s.MSSV,
                    HoTen = s.HoTen,
                    NgaySinh = s.NgaySinh,
                    GioiTinh = s.GioiTinh,
                    DepartmentId = s.DepartmentId,
                    StatusId = s.StatusId,
                    SchoolYearId = s.SchoolYearId,
                    StudyProgramId = s.StudyProgramId,
                    Email = s.Email,
                    SoDienThoai = s.SoDienThoai,
                    QuocTich = s.QuocTich,
                    IdentificationId = s.IdentificationId,
                    DiaChiNhanThuId = s.DiaChiNhanThuId,
                    DiaChiThuongTruId = s.DiaChiThuongTruId,
                    DiaChiTamTruId = s.DiaChiTamTruId
                })
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
                                 .Include(s => s.DiaChiNhanThu)
                                 .Include(s => s.DiaChiThuongTru)
                                 .Include(s => s.DiaChiTamTru)
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

            // Quy tắc chuyển đổi trạng thái hợp lệ
            var _validStatusTransitions = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 2, 3, 4 } }, // "Đang học" → "Bảo lưu", "Tốt nghiệp", "Đình chỉ"
                { 2, new HashSet<int> { } }, // "Đã tốt nghiệp" → Không thể thay đổi
                { 3, new HashSet<int> { } }, // "Đã thôi học" Không thể thay đổi
                { 4, new HashSet<int> { 1, 4 } }   // "Tạm dừng học" → "Đang học", "Đã thôi học"
            };
            // Định nghĩa ánh xạ StatusId sang tên trạng thái
            var statusNames = new Dictionary<int, string>
            {
                { 1, "Đang học" },
                { 2, "Đã tốt nghiệp" },
                { 3, "Đã thôi học" },
                { 4, "Tạm dừng học" }
            };
            // Kiểm tra xem trạng thái mới có hợp lệ không
            if (existingStudent.StatusId != student.StatusId)
            {
                if (!_validStatusTransitions.TryGetValue(existingStudent.StatusId, out var allowedTransitions) ||
                    !allowedTransitions.Contains(student.StatusId))
                {
                    string oldStatus = statusNames.ContainsKey(existingStudent.StatusId) ? statusNames[existingStudent.StatusId] : "Không xác định";
                    string newStatus = statusNames.ContainsKey(student.StatusId) ? statusNames[student.StatusId] : "Không xác định";

                    return (false, $"Không thể chuyển đổi trạng thái sinh viên từ '{oldStatus}' sang '{newStatus}'.");
                }
            }
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
                existingStudent.Email = student.Email;
                existingStudent.SoDienThoai = student.SoDienThoai;
                existingStudent.DiaChiThuongTruId = student.DiaChiThuongTruId;
                existingStudent.DiaChiTamTruId = student.DiaChiTamTruId;
                existingStudent.DiaChiNhanThuId = student.DiaChiNhanThuId;
                existingStudent.QuocTich = student.QuocTich;
                existingStudent.IdentificationId = student.IdentificationId;

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

        public async Task<(IEnumerable<Student>, int, int)> SearchStudents(StudentFilterModel filters, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10; // Default to 10 students per page

            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrEmpty(filters.Keyword))
            {
                query = query
                     .Where(s => EF.Functions.Collate(
                            s.HoTen, "Latin1_General_CI_AI").Contains(filters.Keyword) ||
                            s.MSSV.Contains(filters.Keyword));
            }

            if (filters.DepartmentId.HasValue)
            {
                query = query.Where(s => s.DepartmentId == filters.DepartmentId);
            }

            var totalStudents = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            var students = await query
                .Select(s => new Student
                {
                    MSSV = s.MSSV,
                    HoTen = s.HoTen,
                    NgaySinh = s.NgaySinh,
                    GioiTinh = s.GioiTinh,
                    DepartmentId = s.DepartmentId,
                    StatusId = s.StatusId,
                    SchoolYearId = s.SchoolYearId,
                    StudyProgramId = s.StudyProgramId,
                    Email = s.Email,
                    SoDienThoai = s.SoDienThoai,
                    QuocTich = s.QuocTich,
                    DiaChiNhanThuId = s.DiaChiNhanThuId,
                    DiaChiThuongTruId = s.DiaChiThuongTruId,
                    DiaChiTamTruId = s.DiaChiTamTruId
                })
                .OrderBy(s => s.MSSV) // Sorting by student ID
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalStudents, totalPages);
        }

    }
}