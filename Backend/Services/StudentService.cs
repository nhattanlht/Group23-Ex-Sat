using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.DTOs;
using System.Text.RegularExpressions;

namespace StudentManagement.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;

        public StudentService(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<(IEnumerable<Student>, int, int)> GetStudents(int page, int pageSize)
        {
            var totalStudents = await _studentRepository.GetStudentsCount();
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            var students = await _studentRepository.GetStudents(page, pageSize);

            return (students, totalStudents, totalPages);
        }

        public async Task<Student> GetStudentById(string id)
        {
            return await _studentRepository.GetStudentById(id);
        }

        public async Task<(bool Success, string Message)> CreateStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.SoDienThoai))
                return (false, "Số điện thoại không được để trống.");

            if (!Regex.IsMatch(student.SoDienThoai, @"^(0[2-9]|84[2-9])\d{8,9}$"))
                return (false, "Số điện thoại không hợp lệ.");

            if (await _studentRepository.StudentExistsByPhoneNumber(student.SoDienThoai))
                return (false, "Số điện thoại đã tồn tại trong hệ thống.");

            if (string.IsNullOrWhiteSpace(student.Email))
                return (false, "Email không được để trống.");

            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Email không hợp lệ.");

            if (await _studentRepository.StudentExistsByEmail(student.Email))
                return (false, "Email đã tồn tại trong hệ thống.");

            try
            {
                await _studentRepository.CreateStudent(student);
                return (true, "Sinh viên được tạo thành công.");
            }
            catch
            {
                return (false, "Đã xảy ra lỗi khi tạo sinh viên.");
            }
        }

        public async Task<(bool Success, string Message)> UpdateStudent(Student student)
        {
            var existingStudent = await _studentRepository.GetStudentById(student.MSSV);

            if (existingStudent == null)
                return (false, "Sinh viên không tồn tại.");

            // Add the status transition validation here
            var validStatusTransitions = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 2, 3, 4 } }, // "Đang học" → "Bảo lưu", "Tốt nghiệp", "Đình chỉ"
                { 2, new HashSet<int> { } }, // "Đã tốt nghiệp" → Không thể thay đổi
                { 3, new HashSet<int> { } }, // "Đã thôi học" Không thể thay đổi
                { 4, new HashSet<int> { 1, 4 } }   // "Tạm dừng học" → "Đang học", "Đã thôi học"
            };

            // Define status names for better readability in error messages
            var statusNames = new Dictionary<int, string>
            {
                { 1, "Đang học" },
                { 2, "Đã tốt nghiệp" },
                { 3, "Đã thôi học" },
                { 4, "Tạm dừng học" }
            };

            // Validate the status transition
            if (existingStudent.StatusId != student.StatusId)
            {
                if (!validStatusTransitions.TryGetValue(existingStudent.StatusId, out var allowedTransitions) ||
                    !allowedTransitions.Contains(student.StatusId))
                {
                    string oldStatus = statusNames.ContainsKey(existingStudent.StatusId) ? statusNames[existingStudent.StatusId] : "Không xác định";
                    string newStatus = statusNames.ContainsKey(student.StatusId) ? statusNames[student.StatusId] : "Không xác định";

                    return (false, $"Không thể chuyển đổi trạng thái sinh viên từ '{oldStatus}' sang '{newStatus}'.");
                }
            }

            // Validate phone number and email as before
            if (string.IsNullOrWhiteSpace(student.SoDienThoai))
                return (false, "Số điện thoại không được để trống.");

            if (!Regex.IsMatch(student.SoDienThoai, @"^(0[2-9]|84[2-9])\d{8,9}$"))
                return (false, "Số điện thoại không hợp lệ.");

            if (await _studentRepository.StudentExistsByPhoneNumber(student.SoDienThoai, student.MSSV))
                return (false, "Số điện thoại đã tồn tại trong hệ thống.");

            if (string.IsNullOrWhiteSpace(student.Email))
                return (false, "Email không được để trống.");

            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Email không hợp lệ.");

            if (await _studentRepository.StudentExistsByEmail(student.Email, student.MSSV))
                return (false, "Email đã tồn tại trong hệ thống.");

            try
            {
                await _studentRepository.UpdateStudent(student);
                return (true, "Cập nhật thông tin sinh viên thành công.");
            }
            catch
            {
                return (false, "Đã xảy ra lỗi khi cập nhật sinh viên.");
            }
        }

        public async Task<bool> DeleteStudent(string id)
        {
            return await _studentRepository.DeleteStudent(id);
        }

        public async Task<(IEnumerable<Student>, int, int)> SearchStudents(StudentFilterModel filters, int page, int pageSize)
        {
            var students = await _studentRepository.SearchStudents(filters.Keyword, page, pageSize);
            var totalStudents = await _studentRepository.GetStudentsCount();
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            return (students, totalStudents, totalPages);
        }
    }
}