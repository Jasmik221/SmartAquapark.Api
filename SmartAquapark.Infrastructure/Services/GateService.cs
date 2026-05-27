using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;
using SmartAquapark.Domain.Entities;
using SmartAquapark.Domain.Enums;
using SmartAquapark.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Infrastructure.Services;

public class GateService : IGateService
{
    private readonly AquaparkDbContext _context;

    public GateService(AquaparkDbContext context)
    {
        _context = context;
    }

    public async Task<GateScanResponseDto> ScanAsync(GateScanDto dto)
    {
        var wristband = await _context.Wristbands
            .Include(x => x.Ticket)
            .FirstOrDefaultAsync(x => x.QrCode == dto.QrCode);

        if (wristband == null)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Wristband not found."
            };
        }

        if (wristband.Status != WristbandStatus.Active)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Wristband is not active."
            };
        }

        if (wristband.Ticket.Status != TicketStatus.Active)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Ticket is not active."
            };
        }

        if (wristband.Ticket.ValidUntil < DateTime.UtcNow)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Ticket has expired."
            };
        }

        var zone = await _context.Zones.FindAsync(dto.ZoneId);

        if (zone == null)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Zone not found."
            };
        }

        if (!zone.IsActive)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Zone is not active."
            };
        }

        if (zone.CurrentPeopleCount >= zone.CapacityLimit)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Zone capacity limit reached."
            };
        }

        zone.CurrentPeopleCount++;

        var visit = new ZoneVisit
        {
            WristbandId = wristband.Id,
            ZoneId = zone.Id,
            EnteredAt = DateTime.UtcNow
        };

        _context.ZoneVisits.Add(visit);

        await _context.SaveChangesAsync();

        return new GateScanResponseDto
        {
            AccessGranted = true,
            Message = "Access granted."
        };
    }
    public async Task<GateScanResponseDto> ExitAsync(GateExitDto dto)
    {
        var wristband = await _context.Wristbands
            .Include(x => x.Ticket)
            .FirstOrDefaultAsync(x => x.QrCode == dto.QrCode);

        if (wristband == null)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Wristband not found."
            };
        }

        if (wristband.Status != WristbandStatus.Active)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Wristband is not active."
            };
        }

        var zone = await _context.Zones.FindAsync(dto.ZoneId);

        if (zone == null)
        {
            return new GateScanResponseDto
            {
                AccessGranted = false,
                Message = "Zone not found."
            };
        }

        if (zone.CurrentPeopleCount > 0)
            zone.CurrentPeopleCount--;

        if (dto.FinishWristband)
        {
            wristband.Status = WristbandStatus.Finished;
            wristband.ExitTime = DateTime.UtcNow;
        }

        var activeVisit = await _context.ZoneVisits
    .Where(x =>
        x.WristbandId == wristband.Id &&
        x.ZoneId == zone.Id &&
        x.ExitedAt == null)
    .OrderByDescending(x => x.EnteredAt)
    .FirstOrDefaultAsync();

        if (activeVisit != null)
        {
            activeVisit.ExitedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return new GateScanResponseDto
        {
            AccessGranted = true,
            Message = dto.FinishWristband
                ? "Exit granted. Wristband finished."
                : "Exit granted."
        };
    }
}