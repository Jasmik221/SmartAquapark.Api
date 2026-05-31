using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAquapark.Application.Interfaces;

namespace SmartAquapark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitsController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var visits = await _visitService.GetAllAsync();

        return Ok(visits);
    }

    [HttpGet("wristband/{wristbandId}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetByWristband(int wristbandId)
    {
        var visits = await _visitService.GetByWristbandIdAsync(wristbandId);

        return Ok(visits);
    }

    [HttpGet("zone/{zoneId}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetByZone(int zoneId)
    {
        var visits = await _visitService.GetByZoneIdAsync(zoneId);

        return Ok(visits);
    }
}