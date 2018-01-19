using FluentValidation.Attributes;
using Microsoft.AspNetCore.Identity;
using RSService.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.ViewModels
{
    public class UserViewModel
    { 
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public int? DepartmentId { get; set; }

        public List<int> UserRole { get; set; }

        public string ResetPassCode { get; set; }

        public DateTime DateExpire { get; set; }

    }
}
