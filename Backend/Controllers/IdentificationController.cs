using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

[Route("api/[controller]")]
[ApiController]
public class IdentificationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IdentificationController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateIdentification([FromBody] Identification identification)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Identifications.Add(identification);
        await _context.SaveChangesAsync();

        return Ok(identification);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Identification>> GetIdentificationById(int id)
    {
        var identification = await _context.Identifications.FindAsync(id);
        if (identification == null) return NotFound();
        return Ok(identification);
    }
}