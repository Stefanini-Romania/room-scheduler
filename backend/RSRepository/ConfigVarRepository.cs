using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSRepository
{
    public class ConfigVarRepository : IConfigVarRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<ConfigVar> configVar;

        public ConfigVarRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            configVar = context.Set<ConfigVar>();
        }

        public ConfigVar GetSessionTimeSpan()
        {
            return configVar.FirstOrDefault(v => v.VarName == "SessionTimeSpan");
        }

    }
}
