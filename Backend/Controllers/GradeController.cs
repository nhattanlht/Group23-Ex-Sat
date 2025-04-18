using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Text;

namespace StudentManagement.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly GradeService _service;

        public GradeController(GradeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{MSSV}/{classId}")]
        public async Task<IActionResult> GetById(string MSSV, string classId)
        {
            var result = await _service.GetByIdAsync(MSSV, classId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("export/{MSSV}")]
        public async Task<IActionResult> ExportStudentGrades(string MSSV)
        {
            var grades = (await _service.GetAllAsync())
                         .Where(g => g.MSSV == MSSV)
                         .ToList();

            if (!grades.Any()) return NotFound();

            var csv = new StringBuilder();

            // Header
            csv.AppendLine("\uFEFFBảng điểm sinh viên");
            csv.AppendLine($"Mã Sinh Viên: {MSSV}");
            csv.AppendLine($"Họ Tên: {grades.First().Student.HoTen}");
            csv.AppendLine("\uFEFFMôn Học,Số Tín Chỉ,Điểm,Xếp Loại,GPA");

            // Rows
            foreach (var grade in grades)
            {
                csv.AppendLine($"\"{grade.CourseName}\",{grade.Credit},{grade.Score},{grade.GradeLetter},{grade.GPA}");
            }

            var fileName = $"BangDiem_{MSSV}.csv";
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());

            return File(bytes, "text/csv", fileName);
        }


        [HttpPost]
        public async Task<IActionResult> Create(GradeCreateDTO dto)
        {
            var grade = new Grade
            {
                MSSV = dto.MSSV,
                ClassId = dto.ClassId,
                Score = dto.Score,
                GradeLetter = dto.GradeLetter,
                GPA = dto.GPA
            };
            await _service.AddAsync(grade);
            return Ok();
        }

        [HttpPut("{MSSV}/{classId}")]
        public async Task<IActionResult> Update(string MSSV, string classId, [FromBody] GradeUpdateDTO dto)
        {
            var grade = new Grade
            {
                MSSV = MSSV,
                ClassId = classId,
                Score = dto.Score,
                GradeLetter = dto.GradeLetter,
                GPA = dto.GPA
            };

            await _service.UpdateAsync(grade);
            return Ok();
        }

        [HttpDelete("{MSSV}/{classId}")]
        public async Task<IActionResult> Delete(string MSSV, string classId)
        {
            await _service.DeleteAsync(MSSV, classId);
            return Ok();
        }
    }

}