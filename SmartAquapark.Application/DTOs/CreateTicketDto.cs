using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs;

public class CreateTicketDto
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int NumberOfPeople { get; set; } = 1;

    public DateTime ValidUntil { get; set; }
}