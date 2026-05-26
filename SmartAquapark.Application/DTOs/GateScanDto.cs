using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs;

public class GateScanDto
{
    public string QrCode { get; set; } = string.Empty;

    public int ZoneId { get; set; }
}