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
        public int AvailabilityType { get; set; }
        public int RoomId { get; set; }
        public int HostId { get; set; }
        public int Occurrence { get; set; }
        public int Status { get; set; }

        public User Host { get; set; }
        public Room Room { get; set; }

        public Availability()
        {
        }

        public Availability(DateTime startDate, DateTime endDate, int availabilityType, int roomId, int hostId, int occurrence)
        {
            StartDate = startDate;
            EndDate = endDate;
            AvailabilityType = availabilityType;
            RoomId = roomId;
            HostId = hostId;
            Occurrence = occurrence;
            Status = (int)AvailabilityStatusEnum.active;


        }
    }

    public enum AvailabilityStatusEnum
    {
        active = 0,
        inactive = 1
    }

}
