using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;
using SmartAquapark.Domain.Entities;
using SmartAquapark.Domain.Enums;
using SmartAquapark.Infrastructure.Persistence;

namespace SmartAquapark.Infrastructure.Services;

public class TicketService : ITicketService
{
    private readonly AquaparkDbContext _context;

    public TicketService(AquaparkDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketResponseDto>> GetAllAsync()
    {
        return await _context.Tickets
            .Select(x => new TicketResponseDto
            {
                Id = x.Id,
                VerificationCode = x.VerificationCode,
                CreatedAt = x.CreatedAt,
                ValidUntil = x.ValidUntil,
                Status = x.Status,
                Name = x.Name,
                Price = x.Price,
                NumberOfPeople = x.NumberOfPeople
            })
            .ToListAsync();
    }

    public async Task<TicketResponseDto?> GetByIdAsync(int id)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(x => x.Id == id);

        if (ticket == null)
            return null;

        return new TicketResponseDto
        {
            Id = ticket.Id,
            VerificationCode = ticket.VerificationCode,
            CreatedAt = ticket.CreatedAt,
            ValidUntil = ticket.ValidUntil,
            Status = ticket.Status,
            Name = ticket.Name,
            Price = ticket.Price,
            NumberOfPeople = ticket.NumberOfPeople
        };
    }

    public async Task<TicketResponseDto> CreateAsync(CreateTicketDto dto)
    {
        var ticket = new Ticket
        {
            VerificationCode = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            ValidUntil = DateTime.SpecifyKind(dto.ValidUntil, DateTimeKind.Utc),
            Status = TicketStatus.New,
            Name = dto.Name,
            Price = dto.Price,
            NumberOfPeople = dto.NumberOfPeople
        };

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync();

        return new TicketResponseDto
        {
            Id = ticket.Id,
            VerificationCode = ticket.VerificationCode,
            CreatedAt = ticket.CreatedAt,
            ValidUntil = ticket.ValidUntil,
            Status = ticket.Status,
            Name = ticket.Name,
            Price = ticket.Price,
            NumberOfPeople = ticket.NumberOfPeople
        };
    }

    public async Task<bool> ActivateAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
            return false;

        ticket.Status = TicketStatus.Active;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UseAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
            return false;

        ticket.Status = TicketStatus.Used;

        await _context.SaveChangesAsync();

        return true;
    }
}