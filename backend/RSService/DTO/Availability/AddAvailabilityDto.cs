﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AddAvailabilityDto : AvailabilityDto
    {
        public int[] DaysOfWeek { get; set; }
    }
}
