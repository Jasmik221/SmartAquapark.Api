using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Application.Interfaces;
using SmartAquapark.Domain.Entities;
using SmartAquapark.Infrastructure.Persistence;

namespace SmartAquapark.Infrastructure.Services;

public class ZoneService : IZoneService
{
    private readonly AquaparkDbContext _context;

    public ZoneService(AquaparkDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ZoneResponseDto>> GetAllAsync()
    {
        var zones = await _context.Zones.ToListAsync();

        return zones.Select(z => new ZoneResponseDto
        {
            Id = z.Id,
            Name = z.Name,
            Description = z.Description,
            CapacityLimit = z.CapacityLimit,
            CurrentPeopleCount = z.CurrentPeopleCount,
            IsActive = z.IsActive
        });
    }

    public async Task<ZoneResponseDto?> GetByIdAsync(int id)
    {
        var zone = await _context.Zones.FindAsync(id);

        if (zone == null)
            return null;

        return new ZoneResponseDto
        {
            Id = zone.Id,
            Name = zone.Name,
            Description = zone.Description,
            CapacityLimit = zone.CapacityLimit,
            CurrentPeopleCount = zone.CurrentPeopleCount,
            IsActive = zone.IsActive
        };
    }

    public async Task<ZoneResponseDto> CreateAsync(CreateZoneDto dto)
    {
        var zone = new Zone
        {
            Name = dto.Name,
            Description = dto.Description,
            CapacityLimit = dto.CapacityLimit
        };

        _context.Zones.Add(zone);
        await _context.SaveChangesAsync();

        return new ZoneResponseDto
        {
            Id = zone.Id,
            Name = zone.Name,
            Description = zone.Description,
            CapacityLimit = zone.CapacityLimit,
            CurrentPeopleCount = zone.CurrentPeopleCount,
            IsActive = zone.IsActive
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateZoneDto dto)
    {
        var zone = await _context.Zones.FindAsync(id);

        if (zone == null)
            return false;

        zone.Name = dto.Name;
        zone.Description = dto.Description;
        zone.CapacityLimit = dto.CapacityLimit;
        zone.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var zone = await _context.Zones.FindAsync(id);

        if (zone == null)
            return false;

        _context.Zones.Remove(zone);
        await _context.SaveChangesAsync();

        return true;
    }
}