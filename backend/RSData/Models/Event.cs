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
        public int? HostId { get; set; }
        public int AttendeeId { get; set; }
        public int EventStatus { get; set; }
        public DateTime DateCreated { get; set; }

        public User Attendee { get; set; }
        public User Host { get; set; }
        public Room Room { get; set; }
        public ICollection<Penalty> Penalty { get; set; }

        public Event(DateTime _StartDate, DateTime _EndDate, int _EventType, int _RoomId, string _Notes, int? _HostID, int _AttendeeId, int _EventStatus, DateTime _DateCreated)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;
            EventType = _EventType;
            RoomId = _RoomId;
            Notes = _Notes;
            HostId = _HostID;
            AttendeeId = _AttendeeId;
            EventStatus = _EventStatus;
            DateCreated = _DateCreated;
        }

    }


    public enum EventStatusEnum
    {
        present = 0,
        absent = 1,
        cancelled = 2,
        waiting = 3,
        absentChecked = 4,
        waitingReminder = 5
    }

    public enum EventTypeEnum
    {
        massage = 0,
        availability = 1
    }
}
