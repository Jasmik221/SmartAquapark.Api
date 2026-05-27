using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;

namespace SmartAquapark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GatesController : ControllerBase
{
    private readonly IGateService _gateService;

    public GatesController(IGateService gateService)
    {
        _gateService = gateService;
    }

    [HttpPost("scan")]
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public async Task<IActionResult> Scan(GateScanDto dto)
    {
        var result = await _gateService.ScanAsync(dto);

        if (!result.AccessGranted)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpPost("exit")]
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public async Task<IActionResult> Exit(GateExitDto dto)
    {
        var result = await _gateService.ExitAsync(dto);

        if (!result.AccessGranted)
            return BadRequest(result);

        return Ok(result);
    }
}