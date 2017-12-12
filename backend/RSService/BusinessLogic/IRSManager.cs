using RSData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IRSManager
    {
        List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] hostId, int[] roomId);

        List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] roomId);

        double GetTimeSpan(DateTime start, DateTime end);

        int GetAvailableTime(int attendeeId, DateTime startDate);

        bool CanCancel(DateTime startDate, DateTime endDate, int roomId, int attendee);

        bool CheckAvailability(DateTime startDate, DateTime endDate, int roomId);

        void CheckPenalty(DateTime startDate, int eventId, int attendeeId, int roomId);

        bool HasPenalty(int attendeeId, DateTime newDate, int roomId);

        bool HourCheck(DateTime startDate, DateTime endDate, int roomId);
        
        bool IsUniqueUserName(String username);

        bool IsUniqueEmail(String email);

        bool IsValidRole(List<int> userRole);

        bool IsActiveUser(String username);


    }
}
