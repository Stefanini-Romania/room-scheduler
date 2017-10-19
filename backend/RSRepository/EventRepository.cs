using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System.Linq;

namespace RSRepository
{
    public class EventRepository : IEventRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<Event> events;

        public EventRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            events = context.Set<Event>();
        }


        public IEnumerable<Event> GetEvents()
        {
            return events.AsEnumerable();
        }

        public Event GetEventById(int id)
        {
            return events.SingleOrDefault(s => s.Id == id);
        }

        public void AddEvent(Event _event)
        {
            if (_event == null)
            {
                throw new ArgumentNullException("Add a null event");
            }
            events.Add(_event);
            context.SaveChanges();
        }

        public void UpdateEvent(Event _event)
        {
            if (_event == null)
            {
                throw new ArgumentNullException("Update a null event");
            }
            context.SaveChanges();
        }

        public void DeleteEvent(Event _event)
        {
            if (_event == null)
            {
                throw new ArgumentNullException("Delete null event");
            }
            events.Remove(_event);
            context.SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        
    }
}
