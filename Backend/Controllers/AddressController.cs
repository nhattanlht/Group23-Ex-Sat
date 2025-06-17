using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressService _service;
    private readonly ILogger<AddressController> _logger;

    public AddressController(IAddressService service, ILogger<AddressController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress([FromBody] Address address)
    {
        _logger.LogInformation("Received request to create a new address.");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid address data received.");
            return BadRequest(
                new
                {
                    data = ModelState,
                    message = "Địa chỉ không hợp lệ.",
                    status = "Error",
                }
            );
        }

        try
        {
            await _service.CreateAddressAsync(address);
            _logger.LogInformation("Address created successfully with ID {Id}.", address.Id);
            return Ok(
                new
                {
                    data = address,
                    message = "Địa chỉ đã được tạo thành công.",
                    status = "Success",
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating address.");
            return StatusCode(
                500,
                new
                {
                    data = new { },
                    errors = ex.Message,
                    message = "Lỗi máy chủ cục bộ.",
                    status = "Error",
                }
            );
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> GetAddressById(int id)
    {
        _logger.LogInformation("Fetching address with ID {Id}.", id);

        var address = await _service.GetAddressByIdAsync(id);
        if (address == null)
        {
            _logger.LogWarning("Address with ID {Id} not found.", id);
            return NotFound(
                new
                {
                    data = id,
                    message = "Địa chỉ không tồn tại.",
                    status = "NotFound",
                }
            );
        }

        _logger.LogInformation("Address with ID {Id} retrieved successfully.", id);
        return Ok(
            new
            {
                data = address,
                message = "Địa chỉ đã được tìm thấy.",
                status = "Success",
            }
        );
    }
}