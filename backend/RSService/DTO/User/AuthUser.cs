using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AuthUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? DepartmentId { get; set; }

        public List<int> UserRole { get; set; }

        public List<int> Penalty { get; set; }

        public bool? IsActive { get; set; }

    }
}
