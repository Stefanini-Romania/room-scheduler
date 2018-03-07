using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class SettingsParameterService : ISettingsParameterService
    {
 
        public bool IsNumber(string value)
        {

            if (Double.TryParse(value, out double number))
            {
                return true;
            }
            return false;
        }

        public bool IsGoodReminderTime(string value)
        {

            if (Int32.TryParse(value, out int nr))
            {
                if (nr >= 10 && nr <= 60)
                    return true;
            }
            return false;
        }

        public bool IsGoodSessionTime(string value)
        {

            if (Int32.TryParse(value, out int nr))
            {
                if (nr >= 1 && nr <= 60)
                    return true;
            }
            return false;
        }


        

    }
}
