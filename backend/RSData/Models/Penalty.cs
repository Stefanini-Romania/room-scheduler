using System;
using System.Collections.Generic;

namespace RSData.Models
{

    public partial class Penalty : BaseEntity
    {
        //public int PenaltyId { get; set; }
        public int AttendeeId { get; set; }
        public int EventId { get; set; }
        public DateTime Date { get; set; }
        public int RoomId { get; set; }


        public Penalty(int _attenteeid, int _eventId, DateTime _date, int _roomid)
        {
            AttendeeId = _attenteeid;
            EventId = _eventId;
            Date = _date;
            RoomId = _roomid;
        }

        public User Attendee { get; set; }
        public Event Event { get; set; }
        public Room Room { get; set; }
    }

}
