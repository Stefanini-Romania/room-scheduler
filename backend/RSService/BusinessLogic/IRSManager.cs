using RSData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IRSManager
    {
        List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] roomId, int?[] hostId);

        List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] roomId);

        double GetTimeSpan(DateTime start, DateTime end);

        int GetAvailableTime(int attendeeId, DateTime startDate);

        bool CheckAvailability(DateTime startDate, DateTime endDate, int roomId);

        void AddPenalty(DateTime startDate, int eventId, int attendeeId, int roomId);

        bool HasPenalty(int attendeeId, DateTime newDate, int roomId);

        bool HourCheck(DateTime startDate, DateTime endDate, int roomId);

        bool IsUniqueEmail(String email);

        bool IsUniqueEmailEdit(String email, int userId);

        bool IsValidRole(List<int> userRole);

        bool IsUniqueRoom(String name, String location, int roomid);

        bool IsActiveUser(String email);


    }
}
