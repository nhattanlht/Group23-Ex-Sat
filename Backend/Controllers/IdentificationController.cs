using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement;
using Microsoft.Extensions.Localization;

[Route("api/[controller]")]
[ApiController]
public class IdentificationController : ControllerBase
{
    private readonly IIdentificationService _service;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public IdentificationController(IIdentificationService service, IStringLocalizer<SharedResource> localizer)
    {
        _service = service;
        _localizer = localizer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateIdentification([FromBody] Identification identification)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                new
                {
                    data = ModelState,
                    message = _localizer["InvalidIdentificationData"].Value,
                    status = "Error",
                }
            );
        try
        {
            var result = await _service.CreateIdentificationAsync(identification);
            return Ok(
                new
                {
                    data = result,
                    message = _localizer["CreateIdentificationSuccess"].Value,
                    status = "Success",
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    data = new { },
                    message = _localizer["CreateIdentificationError"].Value,
                    // errors = ex.Message,
                    status = "Error",
                }
            );
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIdentificationById(int id)
    {
        var result = await _service.GetIdentificationByIdAsync(id);
        if (result == null)
            return NotFound(
                new
                {
                    data = id,
                    message = _localizer["IdentificationNotFound"].Value,
                    status = "NotFound",
                }
            );

        return Ok(
            new
            {
                data = result,
                message = _localizer["GetIdentificationSuccess"].Value,
                status = "Success",
            }
        );
    }
}
