﻿using FluentValidation.Attributes;
using RSService.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class AddUserDto
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public int? DepartmentId { get; set; }

        public List<int> UserRole { get; set; }
    }
}
