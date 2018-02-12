using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IEventRepository
    {
        List<Event> GetEvents();

        List<Event> GetEvents(DateTime startDate, DateTime endDate, int?[] roomId, int?[] hostId);

        List<Event> GetEvents(DateTime startDate, DateTime endDate, int?[] roomId);

        List<Event> GetEventsByUser(int attendeeId);

        List<Event> GetEventsByDateTimeNow(int value);     

        List<Event> GetPastEventsByUser(DateTime date, int attendeeId, int roomId);

        List<Event> GetFutureEvents(DateTime date, int attendeeId, int roomId);

        List<Event> GetEventsByRoom(DateTime startDate, DateTime endDate, int roomId);

        List<Event> GetEventsByDay(DateTime date, int userId);

        Event GetEventById(int id);

        void AddEvent(Event _event);

        //void UpdateEvent(Event _event);
    }
}
