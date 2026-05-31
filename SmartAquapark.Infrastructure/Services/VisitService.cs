using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;
using SmartAquapark.Infrastructure.Persistence;

namespace SmartAquapark.Infrastructure.Services;

public class VisitService : IVisitService
{
    private readonly AquaparkDbContext _context;

    public VisitService(AquaparkDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ZoneVisitResponseDto>> GetAllAsync()
    {
        return await _context.ZoneVisits
            .Include(x => x.Wristband)
            .Include(x => x.Zone)
            .OrderByDescending(x => x.EnteredAt)
            .Select(x => new ZoneVisitResponseDto
            {
                Id = x.Id,
                WristbandId = x.WristbandId,
                WristbandQrCode = x.Wristband.QrCode,
                ZoneId = x.ZoneId,
                ZoneName = x.Zone.Name,
                EnteredAt = x.EnteredAt,
                ExitedAt = x.ExitedAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ZoneVisitResponseDto>> GetByWristbandIdAsync(int wristbandId)
    {
        return await _context.ZoneVisits
            .Include(x => x.Wristband)
            .Include(x => x.Zone)
            .Where(x => x.WristbandId == wristbandId)
            .OrderByDescending(x => x.EnteredAt)
            .Select(x => new ZoneVisitResponseDto
            {
                Id = x.Id,
                WristbandId = x.WristbandId,
                WristbandQrCode = x.Wristband.QrCode,
                ZoneId = x.ZoneId,
                ZoneName = x.Zone.Name,
                EnteredAt = x.EnteredAt,
                ExitedAt = x.ExitedAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ZoneVisitResponseDto>> GetByZoneIdAsync(int zoneId)
    {
        return await _context.ZoneVisits
            .Include(x => x.Wristband)
            .Include(x => x.Zone)
            .Where(x => x.ZoneId == zoneId)
            .OrderByDescending(x => x.EnteredAt)
            .Select(x => new ZoneVisitResponseDto
            {
                Id = x.Id,
                WristbandId = x.WristbandId,
                WristbandQrCode = x.Wristband.QrCode,
                ZoneId = x.ZoneId,
                ZoneName = x.Zone.Name,
                EnteredAt = x.EnteredAt,
                ExitedAt = x.ExitedAt
            })
            .ToListAsync();
    }
}