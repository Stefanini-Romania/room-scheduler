﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AvailabilityDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailabilityType { get; set; }
        public int? RoomId { get; set; }
        public string HostName { get; set; }
        public int HostId { get; set; }
    }
}
