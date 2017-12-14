using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
   public interface ITimeSlotRepository
    {
        List<TimeSlot> GetTimeSlots();
        TimeSlot GetTimeSlotById(int id);
        void AddTimeSlot(TimeSlot _timeslot);
        void UpdateTimeSlot(TimeSlot _timeslot);
        void DeleteTimeSlot(TimeSlot _timeslot);
        void SaveChanges();
    }
}
