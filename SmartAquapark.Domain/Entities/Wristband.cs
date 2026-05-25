using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Domain.Enums;

namespace SmartAquapark.Domain.Entities;

public class Wristband
{
    public int Id { get; set; }

    public string QrCode { get; set; } = string.Empty;

    public DateTime ActivatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExitTime { get; set; }

    public WristbandStatus Status { get; set; } = WristbandStatus.Active;

    public int TicketId { get; set; }

    public Ticket Ticket { get; set; } = null!;
}
