using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs;

public class AssignWristbandDto
{
    public int TicketId { get; set; }

    public string QrCode { get; set; } = string.Empty;
}
