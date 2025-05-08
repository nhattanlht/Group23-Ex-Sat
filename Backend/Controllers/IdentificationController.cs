using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

[Route("api/[controller]")]
[ApiController]
public class IdentificationController : ControllerBase
{
    private readonly IIdentificationService _service;

    public IdentificationController(IIdentificationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateIdentification([FromBody] Identification identification)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateIdentificationAsync(identification);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIdentificationById(int id)
    {
        var result = await _service.GetIdentificationByIdAsync(id);
        if (result == null)
            return NotFound(new { message = "Không tìm thấy CMND/CCCD!" });

        return Ok(result);
    }
}