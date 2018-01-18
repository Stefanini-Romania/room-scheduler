using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class Settings : BaseEntity
    {
        public string VarName { get; set; }
        public string Value { get; set; }
    }
}
