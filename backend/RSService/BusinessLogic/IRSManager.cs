using RSData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IRSManager
    {
        IEnumerable<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] hostId, int[] roomId);

        IEnumerable<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] roomId);

        double GetTimeSpan(DateTime start, DateTime end);

        int GetAvailableTime(int attendeeId, DateTime startDate);

        bool CanCancel(DateTime startDate, DateTime endDate, int roomId, int attendee);

        bool CheckAvailability(DateTime startDate, DateTime endDate, int roomId);

        //void CheckPenalty(DateTime startDate, int eventId, int attendeeId);

        bool HasPenalty(int attendeeId, DateTime newDate);
    }
}
