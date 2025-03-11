using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sinh viên
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                                         .Include(s => s.Department)
                                         .Include(s => s.Status)
                                         .ToListAsync();
            return View(students);
        }

        // Hiển thị form thêm sinh viên
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Statuses = _context.StudentStatuses.ToList();
            return View();
        }

        // Xử lý thêm sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            //debug
            // Console.WriteLine($"MSSV: {student.MSSV}");
            // Console.WriteLine($"Họ Tên: {student.HoTen}");
            // Console.WriteLine($"Ngày Sinh: {student.NgaySinh}");
            // Console.WriteLine($"Giới Tính: {student.GioiTinh}");
            // Console.WriteLine($"Khoa: {student.DepartmentId}");
            // Console.WriteLine($"Khóa Học: {student.KhoaHoc}");
            // Console.WriteLine($"Chương Trình: {student.ChuongTrinh}");
            // Console.WriteLine($"Địa Chỉ: {student.DiaChi}");
            // Console.WriteLine($"Email: {student.Email}");
            // Console.WriteLine($"SĐT: {student.SoDienThoai}");
            // Console.WriteLine($"Trạng Thái: {student.StatusId}");
            // if (!ModelState.IsValid)
            // {
            //     Console.WriteLine("ModelState không hợp lệ. Danh sách lỗi:");
            //     foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //     {
            //         Console.WriteLine($"- {error.ErrorMessage}");
            //     }

            //     ViewBag.Departments = _context.Departments.ToList();
            //     ViewBag.Statuses = _context.StudentStatuses.ToList();
            //     return View(student);
            // }

            try
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lưu dữ liệu: {ex.Message}");
                return View(student); // Trả lại view để nhập lại dữ liệu
            }
        }

        // Hiển thị form sửa sinh viên
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Statuses = _context.StudentStatuses.ToList();
            return View(student);
        }

        // Xử lý cập nhật sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Student student)
        {
            if (id != student.MSSV) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // Xác nhận xóa sinh viên
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(m => m.MSSV == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // Xử lý xóa sinh viên
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
