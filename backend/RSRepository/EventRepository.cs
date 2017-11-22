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
            events = context.Event;
        }

        public List<Event> GetEvents()
        {
            return events.ToList();
        }

        public List<Event> GetEvents(DateTime startDate, DateTime endDate, int[] roomId, int[] hostId)
        {
            return events.Where(e => e.StartDate >= startDate)
                         .Where(e => e.StartDate <= endDate)
                         .Where(e => roomId.Contains(e.RoomId))
                         .Where(e => hostId.Contains(e.HostId)) 
                         .Where(e => e.EventStatus == (int)EventStatusEnum.waiting || e.EventStatus == (int)EventStatusEnum.present)
                         .ToList();
        }

        public List<Event> GetEvents(DateTime startDate, DateTime endDate, int[] roomId)
        {
            return events.Where(e => e.StartDate >= startDate)
                         .Where(e => e.StartDate <= endDate)
                         .Where(e => roomId.Contains(e.RoomId))
                         .Where(e => e.EventStatus == (int)EventStatusEnum.waiting || e.EventStatus == (int)EventStatusEnum.present)
                         .ToList();
        }

        public List<Event> GetPastEventsByUser(DateTime date, int attendeeId, int roomId)
        {
            return events.Where(e => e.StartDate > date.AddDays(-30))       // Last 30 days
                         .Where(e => e.AttendeeId == attendeeId)
                         .Where(e => e.RoomId == roomId)
                         .Where(ev => ev.EventStatus == (int)EventStatusEnum.absent)
                         .ToList();
        }

        public List<Event> GetFutureEvents(DateTime date, int attendeeId, int roomId)
        {
            return events.Where(e => e.StartDate <= date.AddDays(15))
                         .Where(e => e.StartDate > date)
                         .Where(e => e.AttendeeId == attendeeId)
                         .Where(e => e.RoomId == roomId).ToList();
        }

        public List<Event> GetEventsByRoom(DateTime startDate, DateTime endDate, int roomId)
        {
            return events.Where(e => e.StartDate >= startDate)
                         .Where(e => e.StartDate <= endDate)
                         .Where(e => e.RoomId == roomId)
                         .ToList();
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
        }


        //public void UpdateEvent(Event _event)
        //{
        //    if (_event == null)
        //    {
        //        throw new ArgumentNullException("Update a null event");
        //    }
        //}



        //public void DeleteEvent(int eventID)
        //{
        //    Event _event = events.Find(eventID);
        //    events.Remove(_event);
        //}

        
    }
}
