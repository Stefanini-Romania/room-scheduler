using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Event
    {
        public Event()
        {
            Penalty = new HashSet<Penalty>();
        }

        public int EventId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EventType { get; set; }
        public int RoomId { get; set; }
        public string Notes { get; set; }
        public int HostId { get; set; }
        public int AttendeeId { get; set; }
        public int EventStatus { get; set; }
        public DateTime DateCreated { get; set; }

        public User Attendee { get; set; }
        public User Host { get; set; }
        public Room Room { get; set; }
        public ICollection<Penalty> Penalty { get; set; }
    }
}
