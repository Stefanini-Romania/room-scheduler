using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

       // public int RoleId { get; set; }
        public string Name { get; set; }

        public ICollection<UserRole> UserRole { get; set; }
    }
}
