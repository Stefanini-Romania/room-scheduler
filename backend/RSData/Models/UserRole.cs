using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }


 public enum UserRoleEnum
    {
        attendee=1,
        host=2,
        admin=3
    }
 }
