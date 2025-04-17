using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

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

        [HttpGet("{studentId}/{classId}")]
        public async Task<IActionResult> GetById(string studentId, string classId)
        {
            var result = await _service.GetByIdAsync(studentId, classId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Grade grade)
        {
            await _service.AddAsync(grade);
            return Ok();
        }

        [HttpPut("{studentId}/{classId}")]
        public async Task<IActionResult> Update(string studentId, string classId, Grade grade)
        {
            if (studentId != grade.StudentId || classId != grade.ClassId) return BadRequest();
            await _service.UpdateAsync(grade);
            return Ok();
        }

        [HttpDelete("{studentId}/{classId}")]
        public async Task<IActionResult> Delete(string studentId, string classId)
        {
            await _service.DeleteAsync(studentId, classId);
            return Ok();
        }
    }

}