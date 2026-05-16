using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Domain.Entities;
using SmartAquapark.Infrastructure.Persistence;

namespace SmartAquapark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonesController : ControllerBase
{
    private readonly AquaparkDbContext _context;

    public ZonesController(AquaparkDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var zones = await _context.Zones.ToListAsync();

        return Ok(zones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var zone = await _context.Zones.FindAsync(id);

        if (zone == null)
            return NotFound();

        return Ok(zone);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateZoneDto dto)
    {
        var zone = new Zone
        {
            Name = dto.Name,
            Description = dto.Description,
            CapacityLimit = dto.CapacityLimit
        };

        _context.Zones.Add(zone);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = zone.Id },
            zone);
    }
}