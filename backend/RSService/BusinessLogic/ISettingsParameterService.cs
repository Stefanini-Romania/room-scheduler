using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface ISettingsParameterService
    {
        bool IsNumber(string value);

        bool IsGoodReminderTime(string value);

        bool IsGoodSessionTime(string value);
    }
}
