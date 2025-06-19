using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using Microsoft.Extensions.Localization;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;


        public EnrollmentController(IEnrollmentService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _service.GetAllAsync();
            return Ok(
                new
                {
                    data = enrollments,
                    message = _localizer["GetAllEnrollmentsSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(
                    new
                    {
                        data = id,
                        message = _localizer["EnrollmentNotFound"].Value,
                        status = "NotFound",
                    }
                );
            return Ok(
                new
                {
                    data = result,
                    message = _localizer["GetEnrollmentSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentCreateDto dto)
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
            var hasPrerequisite = await _service.HasPrerequisiteAsync(dto.StudentId, dto.ClassId);
            if (!hasPrerequisite)
            {
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = _localizer["PrerequisiteNotCompletedError"].Value,
                        status = "Error",
                    }
                );
            }
            var isClassFull = await _service.IsClassFullAsync(dto.ClassId);
            if (isClassFull)
            {
                return BadRequest(
                    new
                    {
                        data = dto,
                        message = _localizer["ClassFullError"].Value,
                        status = "Error",
                    }
                );
            }
            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                RegisteredAt = DateTime.Now,
            };

            await _service.AddAsync(enrollment);
            return Ok(
                new
                {
                    data = enrollment,
                    message = _localizer["CreateEnrollmentSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EnrollmentUpdateDto dto)
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
            var enrollment = await _service.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound(
                    new
                    {
                        data = id,
                        message = _localizer["EnrollmentNotFound"].Value,
                        status = "NotFound",
                    }
                );

            if (dto.IsCancelled && !enrollment.IsCancelled)
            {
                var success = await _service.CancelEnrollmentAsync(
                    id,
                    dto.CancelReason ?? "Không có lý do"
                );
                if (!success)
                    return BadRequest(
                        new
                        {
                            data = id,
                            message = _localizer["CancelEnrollmentDeadlineError"].Value,
                            status = "Error",
                        }
                    );

                return Ok(
                    new
                    {
                        data = id,
                        message = _localizer["CancelEnrollmentSuccess"].Value,
                        status = "Success",
                    }
                );
            }

            await _service.UpdateAsync(enrollment);
            return Ok(
                new
                {
                    data = enrollment,
                    message = _localizer["UpdateEnrollmentSuccess"].Value,
                    status = "Success",
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(
                new
                {
                    data = id,
                    message = _localizer["DeleteEnrollmentSuccess"].Value,
                    status = "Success",
                }
            );
        }
    }
}
