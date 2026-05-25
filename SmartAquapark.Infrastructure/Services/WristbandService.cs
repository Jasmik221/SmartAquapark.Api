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

public class WristbandService : IWristbandService
{
    private readonly AquaparkDbContext _context;

    public WristbandService(AquaparkDbContext context)
    {
        _context = context;
    }

    public async Task<WristbandResponseDto?> AssignAsync(AssignWristbandDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(dto.TicketId);

        if (ticket == null)
            return null;

        if (ticket.Status != TicketStatus.Active)
            return null;

        var wristband = new Wristband
        {
            QrCode = dto.QrCode,
            ActivatedAt = DateTime.UtcNow,
            Status = WristbandStatus.Active,
            TicketId = dto.TicketId
        };

        _context.Wristbands.Add(wristband);

        await _context.SaveChangesAsync();

        return new WristbandResponseDto
        {
            Id = wristband.Id,
            QrCode = wristband.QrCode,
            ActivatedAt = wristband.ActivatedAt,
            ExitTime = wristband.ExitTime,
            Status = wristband.Status,
            TicketId = wristband.TicketId
        };
    }

    public async Task<IEnumerable<WristbandResponseDto>> GetActiveAsync()
    {
        return await _context.Wristbands
            .Where(x => x.Status == WristbandStatus.Active)
            .Select(x => new WristbandResponseDto
            {
                Id = x.Id,
                QrCode = x.QrCode,
                ActivatedAt = x.ActivatedAt,
                ExitTime = x.ExitTime,
                Status = x.Status,
                TicketId = x.TicketId
            })
            .ToListAsync();
    }

    public async Task<bool> MarkAsLostAsync(int id)
    {
        var wristband = await _context.Wristbands.FindAsync(id);

        if (wristband == null)
            return false;

        wristband.Status = WristbandStatus.Lost;

        await _context.SaveChangesAsync();

        return true;
    }
}