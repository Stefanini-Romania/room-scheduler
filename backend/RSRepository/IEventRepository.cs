using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetEvents(DateTime startDate, DateTime endDate, int[] roomId, int[] hostId);
        IEnumerable<Event> GetEvents();
        Event GetEventById(int id);
        void AddEvent(Event _event);
        //void UpdateEvent(Event _event);
        //void DeleteEvent(int id);
    }
}
