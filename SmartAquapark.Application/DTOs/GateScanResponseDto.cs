using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs;

public class GateScanResponseDto
{
    public bool AccessGranted { get; set; }

    public string Message { get; set; } = string.Empty;
}