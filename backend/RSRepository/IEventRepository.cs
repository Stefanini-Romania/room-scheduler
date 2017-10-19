using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetEvents();
        Event GetEventById(int id);
        void AddEvent(Event _event);
        void UpdateEvent(Event _event);
        void DeleteEvent(Event _event);
        void SaveChanges();
    }
}
