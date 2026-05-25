using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Domain.Enums;

namespace SmartAquapark.Domain.Entities;

public class Ticket
{
    public int Id { get; set; }

    public string VerificationCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ValidUntil { get; set; }

    public TicketStatus Status { get; set; } = TicketStatus.New;

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int NumberOfPeople { get; set; } = 1;
    public ICollection<Wristband> Wristbands { get; set; } = new List<Wristband>();
}