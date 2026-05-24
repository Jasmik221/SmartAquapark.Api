using System;
using System.Collections.Generic;
using System.Text;
using SmartAquapark.Domain.Enums;

namespace SmartAquapark.Application.DTOs;

public class TicketResponseDto
{
    public int Id { get; set; }

    public string VerificationCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime ValidUntil { get; set; }

    public TicketStatus Status { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int NumberOfPeople { get; set; }
}