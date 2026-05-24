using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Domain.Entities;
using SmartAquapark.Infrastructure.Persistence;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace SmartAquapark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ZonesController : ControllerBase
{
    private readonly IZoneService _zoneService;

    public ZonesController(IZoneService zoneService)
    {
        _zoneService = zoneService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var zones = await _zoneService.GetAllAsync();

        return Ok(zones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var zone = await _zoneService.GetByIdAsync(id);

        if (zone == null)
            return NotFound();

        return Ok(zone);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateZoneDto dto)
    {
        var zone = await _zoneService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = zone.Id },
            zone);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateZoneDto dto)
    {
        var updated = await _zoneService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _zoneService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}