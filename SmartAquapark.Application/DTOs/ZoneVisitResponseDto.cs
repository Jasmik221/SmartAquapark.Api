using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs;

public class ZoneVisitResponseDto
{
    public int Id { get; set; }

    public int WristbandId { get; set; }

    public string WristbandQrCode { get; set; } = string.Empty;

    public int ZoneId { get; set; }

    public string ZoneName { get; set; } = string.Empty;

    public DateTime EnteredAt { get; set; }

    public DateTime? ExitedAt { get; set; }
}