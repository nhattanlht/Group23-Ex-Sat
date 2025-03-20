using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AddressController> _logger;

    public AddressController(ApplicationDbContext context, ILogger<AddressController> logger)
    {
        _context = context;
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
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

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

        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
        {
            _logger.LogWarning("Address with ID {Id} not found.", id);
            return NotFound();
        }

        _logger.LogInformation("Address with ID {Id} retrieved successfully.", id);
        return Ok(address);
    }
}
