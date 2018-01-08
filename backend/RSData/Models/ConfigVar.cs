using System;
using System.Collections.Generic;

namespace RSData.Models
{
    public partial class ConfigVar
    {
        public int VarId { get; set; }
        public string VarName { get; set; }
        public int Value { get; set; }
    }
}
