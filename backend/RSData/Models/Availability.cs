using RSData.Models;
using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Availability : BaseEntity
    {
        //public int AvailabilityId { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public int DayOfWeek { get; set; }
        public int AvailabilityType { get; set; }
        public int? RoomId { get; set; }
        public int HostId { get; set; }

        public User Host { get; set; }
        public Room Room { get; set; }
    }
}
