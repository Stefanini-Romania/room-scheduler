using System;
using System.Collections.Generic;
using System.Text;
using RSData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RSRepository
{
    public class TimeSlotRepository : ITimeSlotRepository
    {

        private RoomPlannerDevContext context;
        private DbSet<TimeSlot> timeslots;

        public TimeSlotRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            timeslots = context.Set<TimeSlot>();
        }

        public void AddTimeSlot(TimeSlot _timeslot)
        {
            if (_timeslot== null)
            {
                throw new ArgumentNullException("Add a null timeslot");
            }
            timeslots.Add(_timeslot);
            context.SaveChanges();
        }

        public void DeleteTimeSlot(TimeSlot _timeslot)
        {
            if (_timeslot == null)
            {
                throw new ArgumentNullException("Delete null timeslot");
            }
            timeslots.Remove(_timeslot);
            context.SaveChanges();
        }

        public TimeSlot GetTimeSlotById(int id)
        {
            return timeslots.SingleOrDefault(s => s.Id == id);
        }

        public List<TimeSlot> GetTimeSlots()
        {
            return timeslots.ToList();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void UpdateTimeSlot(TimeSlot _timeslot)
        {
            if (_timeslot == null)
            {
                throw new ArgumentNullException("Update a null timeslot");
            }
            context.SaveChanges();
        }
    }
}
