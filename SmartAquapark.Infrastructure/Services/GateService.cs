using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;
using SmartAquapark.Domain.Enums;
using SmartAquapark.Infrastructure.Persistence;

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