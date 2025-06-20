using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Text;
using Microsoft.Extensions.Localization;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GradeController(IGradeService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(
                new
                {
                    data = await _service.GetAllAsync(),
                    message = _localizer["GetAllGradesSuccess"].Value,
                    status = "Success",
                }
            );

        [HttpGet("{StudentId}/{classId}")]
        public async Task<IActionResult> GetById(string StudentId, string classId)
        {
            var result = await _service.GetByIdAsync(StudentId, classId);
            if (result == null)
                return NotFound(
                    new
                    {
                        data = new { StudentId, classId },
                        message = _localizer["GradeNotFound"].Value,
                        status = "NotFound",
                    }
                );
            return Ok(
                new
                {
                    data = result,
                    message = _localizer["GetGradeSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpGet("export/{StudentId}")]
        public async Task<IActionResult> ExportStudentGrades(string StudentId)
        {
            var grades = (await _service.GetAllAsync())
                .Where(g => g.StudentId == StudentId)
                .ToList();

            if (!grades.Any())
                return NotFound(
                    new
                    {
                        data = StudentId,
                        message = _localizer["GradesNotFound", StudentId].Value,
                        status = "NotFound",
                    }
                );

            var csv = new StringBuilder();

            // Header
            csv.AppendLine("\uFEFFBảng điểm sinh viên");
            csv.AppendLine($"Mã Sinh Viên: {StudentId}");
            csv.AppendLine($"Họ Tên: {grades.First().Student.FullName}");
            csv.AppendLine("\uFEFFMôn Học,Số Tín Chỉ,Điểm,Xếp Loại,GPA");

            // Rows
            foreach (var grade in grades)
            {
                csv.AppendLine(
                    $"\"{grade.CourseName}\",{grade.Credit},{grade.Score},{grade.GradeLetter},{grade.GPA}"
                );
            }

            var fileName = $"BangDiem_{StudentId}.csv";
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());

            return File(bytes, "text/csv", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GradeCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp =>
                            kvp.Value != null
                                ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                : Array.Empty<string>()
                    );
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = _localizer["InvalidCourseData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            var grade = new Grade
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                Score = dto.Score,
                GradeLetter = dto.GradeLetter,
                GPA = dto.GPA
            };
            await _service.AddAsync(grade);
            return Ok(
                new
                {
                    data = dto,
                    message = _localizer["CreateGradeSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpPut("student/{StudentId}/class/{classId}")]
        public async Task<IActionResult> Update(
            string StudentId,
            string classId,
            [FromBody] GradeUpdateDTO dto
        )
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp =>
                            kvp.Value != null
                                ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                : Array.Empty<string>()
                    );
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = _localizer["InvalidCourseData"].Value,
                        status = "Error",
                        errors,
                    }
                );
            }
            var existingGrade = await _service.GetByIdAsync(StudentId, classId);
            if (existingGrade == null)
            {
                return NotFound(
                    new
                    {
                        data = new { StudentId, classId },
                        message = _localizer["GradeNotFound"].Value,
                        status = "NotFound",
                    }
                );
            }
            existingGrade.Score = dto.Score;
            existingGrade.GradeLetter = dto.GradeLetter;
            existingGrade.GPA = dto.GPA;

            await _service.UpdateAsync(existingGrade);
            return Ok(
                new
                {
                    data = dto,
                    message = _localizer["UpdateGradeSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpDelete("{gradeId}")]
        public async Task<IActionResult> Delete(int gradeId)
        {
            await _service.DeleteAsync(gradeId);
            return Ok(
                new
                {
                    data = gradeId,
                    message = _localizer["DeleteGradeSuccess"].Value,
                    status = "Success",
                }
            );
        }
    }

}