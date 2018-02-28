using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class User : BaseEntity
    {
        public User()
        {
            Availability = new HashSet<Availability>();
            EventAttendee = new HashSet<Event>();
            EventHost = new HashSet<Event>();
            Penalty = new HashSet<Penalty>();
            UserRole = new HashSet<UserRole>();
        }

       // public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public string ResetPassCode { get; set; }
        public DateTime DateExpire { get; set; }



        public User(string _email, string _pass, int? _department, string _firstname, string _lastname, bool? _isactive, DateTime _dateTime)
        {
            Email = _email;
            Password = _pass;
            DepartmentId = _department;
            FirstName = _firstname;
            LastName = _lastname;
            IsActive = _isactive;
            DateExpire = _dateTime;

        }

        public Department Department { get; set; }
        public ICollection<Availability> Availability { get; set; }
        public ICollection<Event> EventAttendee { get; set; }
        public ICollection<Event> EventHost { get; set; }
        public ICollection<Penalty> Penalty { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
    }
}
