using System;
using System.Collections.Generic;

namespace RSData.Models
{

    public partial class Room : BaseEntity
    {
        public Room()
        {
            Availability = new HashSet<Availability>();
            Event = new HashSet<Event>();
            Penalty = new HashSet<Penalty>();
            TimeSlot = new HashSet<TimeSlot>();
        }

        //public int RoomId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool? IsActive { get; set; }



        public Room(string _name, string _location, bool? _isactive)
        {
            Name = _name;
            Location = _location;
            IsActive = _isactive;
        }

        public ICollection<Availability> Availability { get; set; }
        public ICollection<Event> Event { get; set; }
        public ICollection<Penalty> Penalty { get; set; }
        public ICollection<TimeSlot> TimeSlot { get; set; }
    }


}
