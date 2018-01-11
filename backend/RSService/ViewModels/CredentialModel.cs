using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RSService.ViewModels
{
    public class CredentialModel
    {
        public string LoginName { get; set; }

        public string Password { get; set; }
    }
}
