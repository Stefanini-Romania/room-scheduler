using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSRepository
{
    public class SettingsRepository : ISettingsRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<Settings> settings;

        public SettingsRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            settings = context.Set<Settings>();
        }


        public List<Settings> GetSettings()
        {
            return settings.ToList();
        }

        public Settings GetSettingsById(int id)
        {
            return settings.FirstOrDefault(s => s.Id == id);
        }

        public List<Settings> GetValueOfEmailReminderSettings()
        {
            return settings.Where(x => x.VarName.Equals("EmailReminderTime"))
                           .ToList();
        }

        public Settings GetSessionTimeSpan()
        {
            return settings.FirstOrDefault(v => v.VarName == "SessionTimeSpan");
        }

    }
}
