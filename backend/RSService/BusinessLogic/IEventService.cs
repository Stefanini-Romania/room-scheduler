using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IEventService
    {

        double GetTimeSpan(DateTime start, DateTime end);

        int GetAvailableTime(int attendeeId, DateTime startDate);

        bool CheckAvailability(DateTime startDate, DateTime endDate, int roomId);

        bool HourCheck(DateTime startDate, DateTime endDate, int roomId);

        bool IsAvailableTimeSpan(AddEventDto eventDto);

        bool IsGoodTimeSpan(AddEventDto eventDto);

        bool IsGoodStartTime(DateTime? d);

        bool IsGoodEndTime(DateTime? d);

        bool IsGoodDayOfWeek(DateTime? d);

        bool IsInGoodRange(DateTime? d);
    }
}
