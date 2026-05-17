using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs;

public class UpdateZoneDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int CapacityLimit { get; set; }

    public bool IsActive { get; set; }
}