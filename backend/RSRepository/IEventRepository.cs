using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetEvents();

        IEnumerable<Event> GetEvents(DateTime startDate, DateTime endDate, int[] roomId, int[] hostId);

        IEnumerable<Event> GetEvents(DateTime startDate, DateTime endDate, int[] roomId);

        IEnumerable<Event> GetEventsByRoom(DateTime startDate, DateTime endDate, int roomId);

        Event GetEventById(int id);

        void AddEvent(Event _event);

        //void UpdateEvent(Event _event);
        //void DeleteEvent(int id);
    }
}
