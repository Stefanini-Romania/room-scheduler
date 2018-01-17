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
        Settings GetSettingsByName(string name);
    }
}
