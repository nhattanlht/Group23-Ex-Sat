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
        private readonly IGradeService _service;

        public GradeController(IGradeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{StudentId}/{classId}")]
        public async Task<IActionResult> GetById(string StudentId, string classId)
        {
            var result = await _service.GetByIdAsync(StudentId, classId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("export/{StudentId}")]
        public async Task<IActionResult> ExportStudentGrades(string StudentId)
        {
            var grades = (await _service.GetAllAsync())
                         .Where(g => g.StudentId == StudentId)
                         .ToList();

            if (!grades.Any()) return NotFound();

            var csv = new StringBuilder();

            // Header
            csv.AppendLine("\uFEFFBảng điểm sinh viên");
            csv.AppendLine($"Mã Sinh Viên: {StudentId}");
            csv.AppendLine($"Họ Tên: {grades.First().Student.FullName}");
            csv.AppendLine("\uFEFFMôn Học,Số Tín Chỉ,Điểm,Xếp Loại,GPA");

            // Rows
            foreach (var grade in grades)
            {
                csv.AppendLine($"\"{grade.CourseName}\",{grade.Credit},{grade.Score},{grade.GradeLetter},{grade.GPA}");
            }

            var fileName = $"BangDiem_{StudentId}.csv";
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());

            return File(bytes, "text/csv", fileName);
        }


        [HttpPost]
        public async Task<IActionResult> Create(GradeCreateDTO dto)
        {
            var grade = new Grade
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                Score = dto.Score,
                GradeLetter = dto.GradeLetter,
                GPA = dto.GPA
            };
            await _service.AddAsync(grade);
            return Ok();
        }

        [HttpPut("{StudentId}/{classId}")]
        public async Task<IActionResult> Update(string StudentId, string classId, [FromBody] GradeUpdateDTO dto)
        {
            var grade = new Grade
            {
                StudentId = StudentId,
                ClassId = classId,
                Score = dto.Score,
                GradeLetter = dto.GradeLetter,
                GPA = dto.GPA
            };

            await _service.UpdateAsync(grade);
            return Ok();
        }

        [HttpDelete("{gradeId}")]
        public async Task<IActionResult> Delete(int gradeId)
        {
            await _service.DeleteAsync(gradeId);
            return Ok();
        }
    }

}