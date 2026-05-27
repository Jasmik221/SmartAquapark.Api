using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Domain.Entities;

public class ZoneVisit
{
    public int Id { get; set; }

    public int WristbandId { get; set; }

    public Wristband Wristband { get; set; } = null!;

    public int ZoneId { get; set; }

    public Zone Zone { get; set; } = null!;

    public DateTime EnteredAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExitedAt { get; set; }
}