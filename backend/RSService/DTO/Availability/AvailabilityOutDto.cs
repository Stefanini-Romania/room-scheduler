﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AvailabilityOutDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailabilityType { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int Occurrence { get; set; }

        public AvailabilityOutDto(int id, DateTime startDate, DateTime endDate, int availabilityType, int roomId, string roomName, int occurrence)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            AvailabilityType = availabilityType;
            RoomId = roomId;
            RoomName = roomName;
            Occurrence = occurrence;
        }

    }
}
