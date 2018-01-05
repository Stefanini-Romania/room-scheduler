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

        public int GetSessionTimeSpan()
        {
            return configVar.First().SessionTimeSpan;   
        }

        public ConfigVar GetConfigVariables()
        {
            return configVar.First();
        }
    }
}
