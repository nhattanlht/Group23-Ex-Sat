using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AddressController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress([FromBody] Address address)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        return Ok(address);
    }
}