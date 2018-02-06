﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AvailabilityDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailabilityType { get; set; }
        public string RoomName { get; set; }

        public AvailabilityDto(int id, DateTime startDate, DateTime endDate, int availabilityType, string roomName)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            AvailabilityType = availabilityType;
            RoomName = roomName;
        }

    }
}
