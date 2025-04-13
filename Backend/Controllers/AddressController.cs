using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly AddressService _service;
    private readonly ILogger<AddressController> _logger;

    public AddressController(AddressService service, ILogger<AddressController> logger)
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
            return BadRequest(ModelState);
        }

        try
        {
            await _service.CreateAddressAsync(address);
            _logger.LogInformation("Address created successfully with ID {Id}.", address.Id);
            return Ok(address);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating address.");
            return StatusCode(500, "Internal Server Error");
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
            return NotFound();
        }

        _logger.LogInformation("Address with ID {Id} retrieved successfully.", id);
        return Ok(address);
    }
}