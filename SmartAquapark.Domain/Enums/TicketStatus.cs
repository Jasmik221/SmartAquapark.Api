using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Domain.Enums;

public enum TicketStatus
{
    New,
    Active,
    Used,
    Expired,
    Refunded
}