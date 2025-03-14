using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Text.RegularExpressions;


namespace StudentManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        const int nStudents = 10;
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sinh viên
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var totalStudents = await _context.Students.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            var students = await _context.Students
                                         .OrderBy(s => s.MSSV)
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .Include(s => s.Department)
                                         .Include(s => s.Khoa)
                                         .Include(s => s.Program)
                                         .Include(s => s.Status)
                                         .ToListAsync();

            ViewBag.Departments = await _context.Departments.ToListAsync();
            ViewBag.CacKhoa = await _context.CacKhoa.ToListAsync();
            ViewBag.Programs = await _context.Programs.ToListAsync();
            ViewBag.Statuses = await _context.StudentStatuses.ToListAsync();
            ViewBag.Genders = new List<string> { "Nam", "Nữ", "Khác" };
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(students);
        }

        // Xử lý thêm sinh viên qua AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            //  Kiểm tra số điện thoại bị trống
            if (string.IsNullOrWhiteSpace(student.SoDienThoai))
            {
                Console.WriteLine("Số điện thoại bị trống.");
                return BadRequest("Số điện thoại không được để trống.");
            }

            //  Kiểm tra định dạng số điện thoại
            if (!Regex.IsMatch(student.SoDienThoai, @"^(0[2-9]|84[2-9])\d{8,9}$"))
            {
                Console.WriteLine("Số điện thoại không hợp lệ.");
                return BadRequest("Số điện thoại không hợp lệ.");
            }

            //  Kiểm tra trùng số điện thoại
            var existingPhone = await _context.Students.AnyAsync(s => s.SoDienThoai == student.SoDienThoai);
            if (existingPhone)
            {
                Console.WriteLine("Số điện thoại đã tồn tại.");
                return BadRequest("Số điện thoại đã tồn tại.");
            }

            //  Kiểm tra email bị trống
            if (string.IsNullOrWhiteSpace(student.Email))
            {
                Console.WriteLine("Email bị trống.");
                return BadRequest("Email không được để trống.");
            }

            //  Kiểm tra định dạng email
            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Console.WriteLine("Email không hợp lệ.");
                return BadRequest("Email không hợp lệ.");
            }

            //  Kiểm tra trùng email
            var existingEmail = await _context.Students.AnyAsync(s => s.Email == student.Email);
            if (existingEmail)
            {
                Console.WriteLine("Email đã tồn tại.");
                return BadRequest("Email đã tồn tại.");
            }

            try
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lưu dữ liệu: {ex.Message}");
            }
        }

        // Lấy thông tin sinh viên để chỉnh sửa
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid student ID.");
            }

            var student = await _context.Students
                                        .Include(s => s.Department)
                                        .Include(s => s.Khoa)
                                        .Include(s => s.Program)
                                        .Include(s => s.Status)
                                        .FirstOrDefaultAsync(s => s.MSSV == id);

            if (student == null)
            {
                return NotFound("Student not found.");
            }
            Console.WriteLine(student.HoTen); // Kiểm tra dữ liệu trước khi trả về
            return Json(new
            {
                mssv = student.MSSV,
                hoTen = student.HoTen,
                ngaySinh = student.NgaySinh.ToString("yyyy-MM-dd"),
                gioiTinh = student.GioiTinh,
                departmentId = student.DepartmentId,
                statusId = student.StatusId,
                khoaId = student.KhoaId,
                programId = student.ProgramId,
                diaChi = student.DiaChi,
                email = student.Email,
                soDienThoai = student.SoDienThoai
            });
        }

        // Sửa thông tin sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.MSSV);
            if (existingStudent == null)
            {
                return NotFound("Student not found.");
            }

            //  Kiểm tra số điện thoại bị trống
            if (string.IsNullOrWhiteSpace(student.SoDienThoai))
            {
                Console.WriteLine("Số điện thoại bị trống.");
                return BadRequest("Số điện thoại không được để trống.");
            }

            //  Kiểm tra định dạng số điện thoại
            if (!Regex.IsMatch(student.SoDienThoai, @"^(0[2-9]|84[2-9])\d{8,9}$"))
            {
                Console.WriteLine("Số điện thoại không hợp lệ.");
                return BadRequest("Số điện thoại không hợp lệ.");
            }

            //  Kiểm tra số điện thoại có bị trùng với sinh viên khác không
            var phoneExists = await _context.Students
                .AnyAsync(s => s.SoDienThoai == student.SoDienThoai && s.MSSV != student.MSSV);
            if (phoneExists)
            {
                Console.WriteLine("Số điện thoại đã được sử dụng bởi sinh viên khác.");
                return BadRequest("Số điện thoại đã được sử dụng bởi sinh viên khác.");
            }

            //  Kiểm tra email bị trống
            if (string.IsNullOrWhiteSpace(student.Email))
            {
                Console.WriteLine("Email bị trống.");
                return BadRequest("Email không được để trống.");
            }

            //  Kiểm tra định dạng email
            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Console.WriteLine("Email không hợp lệ.");
                return BadRequest("Email không hợp lệ.");
            }

            //  Kiểm tra email có bị trùng với sinh viên khác không
            var emailExists = await _context.Students
                .AnyAsync(s => s.Email == student.Email && s.MSSV != student.MSSV);
            if (emailExists)
            {
                Console.WriteLine("Email đã được sử dụng bởi sinh viên khác.");
                return BadRequest("Email đã được sử dụng bởi sinh viên khác.");
            }

            try
            {
                existingStudent.HoTen = student.HoTen;
                existingStudent.NgaySinh = student.NgaySinh;
                existingStudent.GioiTinh = student.GioiTinh;
                existingStudent.DepartmentId = student.DepartmentId;
                existingStudent.StatusId = student.StatusId;
                existingStudent.KhoaId = student.KhoaId;
                existingStudent.ProgramId = student.ProgramId;
                existingStudent.DiaChi = student.DiaChi;
                existingStudent.Email = student.Email;
                existingStudent.SoDienThoai = student.SoDienThoai;

                _context.Update(existingStudent);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        // Xoá sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi xóa dữ liệu: {ex.Message}");
            }
        }

        // Tìm kiếm sinh viên
        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int page, int pageSize)
        {
            var students = await _context.Students
                .Where(s => s.HoTen.Contains(keyword) || s.MSSV.Contains(keyword))
                .OrderBy(s => s.MSSV)
                .Skip((page - 1) * pageSize)
                .Take(nStudents)
                .Select(s => new
                {
                    s.MSSV,
                    s.HoTen,
                    s.NgaySinh,
                    s.GioiTinh,
                    DepartmentName = s.Department.Name,
                    StatusName = s.Status.Name,
                    Khoa = s.Khoa.Name,
                    ProgramName = s.Program.Name,
                    s.DiaChi,
                    s.Email,
                    s.SoDienThoai
                })
                .ToListAsync();

            return Json(students);
        }


        [HttpGet]
        public async Task<IActionResult> GetStudents(int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = nStudents; // Default to 10 students per page

            var totalStudents = await _context.Students.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            var students = await _context.Students
                .OrderBy(s => s.MSSV) // Sorting by student ID (change if needed)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                students,
                totalStudents,
                totalPages,
                currentPage = page,
                pageSize
            });
        }

    }
}