using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Interfaces;

public interface IWristbandService
{
    Task<WristbandResponseDto?> AssignAsync(AssignWristbandDto dto);

    Task<IEnumerable<WristbandResponseDto>> GetActiveAsync();

    Task<bool> MarkAsLostAsync(int id);
}
