using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Penalty : BaseEntity
    {
        public int AttendeeId { get; set; }
        public int EventId { get; set; }
        public DateTime Date { get; set; }

        public User Attendee { get; set; }
        public Event Event { get; set; }
    }
}
