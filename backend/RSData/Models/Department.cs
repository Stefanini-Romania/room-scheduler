using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Department : BaseEntity
    {
        public Department() : base()
        {
            User = new HashSet<User>();
        }

        public string Name { get; set; }

        public ICollection<User> User { get; set; }
    }
}
