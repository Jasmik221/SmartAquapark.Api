using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Interfaces;

public interface IGateService
{
    Task<GateScanResponseDto> ScanAsync(GateScanDto dto);
    Task<GateScanResponseDto> ExitAsync(GateExitDto dto);
}