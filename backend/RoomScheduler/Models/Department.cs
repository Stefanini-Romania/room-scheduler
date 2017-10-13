using System;
using System.Collections.Generic;

namespace RoomScheduler.Models
{
    public partial class Department
    {
        public Department()
        {
            User = new HashSet<User>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; }

        public ICollection<User> User { get; set; }
    }
}
