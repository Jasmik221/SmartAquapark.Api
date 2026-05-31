using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Interfaces;

public interface IVisitService
{
    Task<IEnumerable<ZoneVisitResponseDto>> GetAllAsync();

    Task<IEnumerable<ZoneVisitResponseDto>> GetByWristbandIdAsync(int wristbandId);

    Task<IEnumerable<ZoneVisitResponseDto>> GetByZoneIdAsync(int zoneId);
}
