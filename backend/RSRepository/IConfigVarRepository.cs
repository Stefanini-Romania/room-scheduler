using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IConfigVarRepository
    {
        ConfigVar GetSessionTimeSpan();
    }
}
