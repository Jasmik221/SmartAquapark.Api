using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;

namespace SmartAquapark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WristbandsController : ControllerBase
{
    private readonly IWristbandService _wristbandService;

    public WristbandsController(IWristbandService wristbandService)
    {
        _wristbandService = wristbandService;
    }

    [HttpPost("assign")]
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public async Task<IActionResult> Assign(AssignWristbandDto dto)
    {
        var wristband = await _wristbandService.AssignAsync(dto);

        if (wristband == null)
            return BadRequest("Ticket does not exist or is not active.");

        return Ok(wristband);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var wristbands = await _wristbandService.GetActiveAsync();

        return Ok(wristbands);
    }

    [HttpPost("{id}/lost")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> MarkAsLost(int id)
    {
        var result = await _wristbandService.MarkAsLostAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}