using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int RoomId { get; set; }
        public int HostId { get; set; }

        public User Host { get; set; }
        public Room Room { get; set; }
    }
}
