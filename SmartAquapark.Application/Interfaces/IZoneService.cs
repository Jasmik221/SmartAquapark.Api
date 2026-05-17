using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Interfaces;

public interface IZoneService
{
    Task<IEnumerable<ZoneResponseDto>> GetAllAsync();

    Task<ZoneResponseDto?> GetByIdAsync(int id);

    Task<ZoneResponseDto> CreateAsync(CreateZoneDto dto);

    Task<bool> UpdateAsync(int id, UpdateZoneDto dto);

    Task<bool> DeleteAsync(int id);
}