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
            return BadRequest(
                new
                {
                    data = ModelState,
                    message = "Dữ liệu CMND/CCCD không hợp lệ.",
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
                    message = "CMND/CCCD đã được tạo thành công.",
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
                    message = "Lỗi khi tạo CMND/CCCD.",
                    errors = ex.Message,
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
                    message = "Không tìm thấy CMND/CCCD!",
                    status = "NotFound",
                }
            );

        return Ok(
            new
            {
                data = result,
                message = "Lấy thông tin CMND/CCCD thành công.",
                status = "Success",
            }
        );
    }
}
