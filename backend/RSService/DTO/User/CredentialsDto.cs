using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RSService.DTO
{
    public class CredentialsDto
    {
        public string LoginName { get; set; }

        public string Password { get; set; }
    }
}
