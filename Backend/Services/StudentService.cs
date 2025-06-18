using System.Text.RegularExpressions;
using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Repositories;
using Microsoft.Extensions.Localization;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public StudentService(IStudentRepository studentRepository, IStringLocalizer<SharedResource> localizer)
        {
            _studentRepository = studentRepository;
            _localizer = localizer;
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
            if (string.IsNullOrWhiteSpace(student.PhoneNumber))
                return (false, _localizer["PhoneNumberRequired"].Value);

            if (await _studentRepository.StudentExistsByPhoneNumber(student.PhoneNumber))
                return (false, _localizer["PhoneNumberExists"].Value);

            if (string.IsNullOrWhiteSpace(student.Email))
                return (false, _localizer["EmailRequired"].Value);

            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, _localizer["EmailInvalid"].Value);

            if (await _studentRepository.StudentExistsByEmail(student.Email))
                return (false, _localizer["EmailExists"].Value);

            try
            {
                await _studentRepository.CreateStudent(student);
                return (true, _localizer["CreateStudentSuccess"].Value);
            }
            catch
            {
                return (false, _localizer["CreateStudentError"].Value);
            }
        }

        public async Task<(bool Success, string Message)> UpdateStudent(Student student)
        {
            var existingStudent = await _studentRepository.GetStudentById(student.StudentId);

            if (existingStudent == null)
                return (false, _localizer["StudentNotFound"].Value);

            // Add the status transition validation here
            var validStatusTransitions = new Dictionary<int, HashSet<int>>
            {
                {
                    1,
                    new HashSet<int> { 2, 3, 4 }
                }, // "Đang học" → "Bảo lưu", "Tốt nghiệp", "Đình chỉ"
                {
                    2,
                    new HashSet<int> { }
                }, // "Đã tốt nghiệp" → Không thể thay đổi
                {
                    3,
                    new HashSet<int> { }
                }, // "Đã thôi học" Không thể thay đổi
                {
                    4,
                    new HashSet<int> { 1, 4 }
                }, // "Tạm dừng học" → "Đang học", "Đã thôi học"
            };

            // Define status names for better readability in error messages
            var statusNames = new Dictionary<int, string>
            {
                { 1, "Đang học" },
                { 2, "Đã tốt nghiệp" },
                { 3, "Đã thôi học" },
                { 4, "Tạm dừng học" },
            };

            // Validate the status transition
            if (existingStudent.StatusId != student.StatusId)
            {
                if (
                    !validStatusTransitions.TryGetValue(
                        existingStudent.StatusId,
                        out var allowedTransitions
                    ) || !allowedTransitions.Contains(student.StatusId)
                )
                {
                    string oldStatus = statusNames.ContainsKey(existingStudent.StatusId)
                        ? statusNames[existingStudent.StatusId]
                        : "Không xác định";
                    string newStatus = statusNames.ContainsKey(student.StatusId)
                        ? statusNames[student.StatusId]
                        : "Không xác định";

                    return (
                        false,
                        $"Không thể chuyển đổi trạng thái sinh viên từ '{oldStatus}' sang '{newStatus}'."
                    );
                }
            }

            // Validate phone number and email as before
            if (string.IsNullOrWhiteSpace(student.PhoneNumber))
                return (false, _localizer["PhoneNumberRequired"].Value);

            if (
                await _studentRepository.StudentExistsByPhoneNumber(
                    student.PhoneNumber,
                    student.StudentId
                )
            )
                return (false, _localizer["PhoneNumberExists"].Value);

            if (string.IsNullOrWhiteSpace(student.Email))
                return (false, _localizer["EmailRequired"].Value);

            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, _localizer["EmailInvalid"].Value);

            if (await _studentRepository.StudentExistsByEmail(student.Email, student.StudentId))
                return (false, _localizer["EmailExists"].Value);

            try
            {
                await _studentRepository.UpdateStudent(student);
                return (true, _localizer["UpdateStudentSuccess"].Value);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> DeleteStudent(string id)
        {
            return await _studentRepository.DeleteStudent(id);
        }

        public async Task<(IEnumerable<Student>, int, int)> SearchStudents(
            StudentFilterModel filters,
            int page,
            int pageSize
        )
        {
            var students = await _studentRepository.SearchStudents(filters, page, pageSize);
            var totalStudents = await _studentRepository.GetStudentsCount();
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            return (students, totalStudents, totalPages);
        }
    }
}
