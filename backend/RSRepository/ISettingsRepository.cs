using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface ISettingsRepository
    {
        Settings GetSessionTimeSpan();

        List<Settings> GetSettings();

        List<Settings> GetValueOfEmailReminderSettings();

        Settings GetSettingsById(int id);
    }
}
