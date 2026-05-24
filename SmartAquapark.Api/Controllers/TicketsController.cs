using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;

namespace SmartAquapark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _ticketService.GetAllAsync();

        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);

        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public async Task<IActionResult> Create(CreateTicketDto dto)
    {
        var ticket = await _ticketService.CreateAsync(dto);

        return Ok(ticket);
    }

    [HttpPost("{id}/activate")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Activate(int id)
    {
        var result = await _ticketService.ActivateAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/use")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Use(int id)
    {
        var result = await _ticketService.UseAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}