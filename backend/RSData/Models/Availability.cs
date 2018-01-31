using RSData.Models;
using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Availability : BaseEntity
    {
        //public int AvailabilityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DayOfWeek { get; set; }
        public int AvailabilityType { get; set; }
        public int? RoomId { get; set; }
        public int HostId { get; set; }
        public int? Occurrence { get; set; }

        public User Host { get; set; }
        public Room Room { get; set; }

        public Availability()
        {
        }

        public Availability(DateTime startDate, DateTime endDate, int availabilityType, int? roomId, int hostId, int? occurrence)
        {
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = (int)startDate.DayOfWeek;
            AvailabilityType = availabilityType;
            RoomId = roomId;
            HostId = hostId;
            Occurrence = occurrence;
            
        }

        public Availability(DateTime startDate, DateTime endDate, int dayOfWeek, int availabilityType, int? roomId, int hostId, int? occurrence)
        {
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = dayOfWeek;
            AvailabilityType = availabilityType;
            RoomId = roomId;
            HostId = hostId;
            Occurrence = occurrence;
        }
    }
}
