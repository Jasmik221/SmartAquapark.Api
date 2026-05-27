using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs;

public class GateExitDto
{
    public string QrCode { get; set; } = string.Empty;

    public int ZoneId { get; set; }

    public bool FinishWristband { get; set; } = false;
}