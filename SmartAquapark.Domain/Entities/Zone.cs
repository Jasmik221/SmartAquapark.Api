using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Domain.Entities
{
    public class Zone
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CapacityLimit { get; set; }

        public int CurrentPeopleCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
