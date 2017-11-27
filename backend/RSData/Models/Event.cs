using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Event : BaseEntity
    {
        public Event()
        {
            Penalty = new HashSet<Penalty>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EventType { get; set; }
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


    public enum EventStatusEnum
    {
        present = 0,
        absent = 1,
        cancelled = 2,
        waiting = 3,
        absentChecked = 4
    }

    public enum EventTypeEnum
    {
        massage = 0,
        availability = 1
    }
}
