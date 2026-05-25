using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Domain.Enums;

namespace SmartAquapark.Application.DTOs;

public class WristbandResponseDto
{
    public int Id { get; set; }

    public string QrCode { get; set; } = string.Empty;

    public DateTime ActivatedAt { get; set; }

    public DateTime? ExitTime { get; set; }

    public WristbandStatus Status { get; set; }

    public int TicketId { get; set; }
}