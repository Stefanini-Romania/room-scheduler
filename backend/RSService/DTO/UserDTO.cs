using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class UserDto
    {

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? DepartmentId { get; set; }
        public List<int> UserRole { get; set; }
        public bool? IsActive { get; set; }
        public int? ResetPassCode { get; set; }
        public DateTime DateExpire { get; set; }
    }
}
