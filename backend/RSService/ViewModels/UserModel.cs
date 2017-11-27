using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.ViewModels
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int? DepartmentId { get; set; }
        public int RoleId { get; set; }
    }
}
