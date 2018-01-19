﻿using System;
using System.Collections.Generic;
using System.Text;
using RSData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RSRepository
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<Availability> availabilities;

        public AvailabilityRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            availabilities = context.Set<Availability>();
        }

        public Availability GetAvailabilityById(int id)
        {
            return availabilities.FirstOrDefault(s => s.Id == id);
        }

        public List<Availability> GetAvailabilities()
        {
            return availabilities.Where(e => e.DayOfWeek == 1)
                                 .Include(e => e.Host)
                                 .ToList();
        }

        public List<Availability> GetAvailabilities(int?[] roomId, int?[] hostId)
        {
            return availabilities.Where(e => roomId.Contains(e.RoomId))
                                 .Where(e => hostId.Contains(e.HostId))
                                 .Include(e => e.Host)
                                 .ToList();
        }

        public List<Availability> GetAvailabilitiesByRoom(DateTime startDate, DateTime endDate, int roomId)
        {
            return availabilities.Where(e => e.StartDate.TimeOfDay <= startDate.TimeOfDay)
                                 .Where(e => e.StartDate.TimeOfDay <= endDate.TimeOfDay)
                                 .Where(e => e.RoomId == roomId)                        
                                 .ToList();
        }

        public List<Availability> GetAvailabilities(int?[] roomId)
        {
            return availabilities.Where(e => roomId.Contains(e.RoomId))
                                 .Include(e => e.Host)
                                 .ToList();
        }

        public List<Availability> GetAvailabilitiesByType(DateTime startDate, DateTime endDate,int roomId)
        {
            return availabilities.Where(e => e.StartDate.TimeOfDay <= startDate.TimeOfDay)
                                 .Where(e => e.StartDate.TimeOfDay <= endDate.TimeOfDay)
                                 .Where(e => e.RoomId == roomId)
                                 .Where(e => e.AvailabilityType==1)
                                 .ToList();
        }

        public List<Availability> GetAvailabilitiesByHour(DateTime startDate, int roomId)
        {
            return availabilities.Where(a => a.RoomId == roomId)
                                 .Where(a => a.StartDate.Hour <= startDate.Hour)
                                 .Where(a => a.EndDate.Hour > startDate.Hour)
                                 .ToList();
        }

        public void AddAvailability(Availability availability)
        {
            availabilities.Add(availability);
        }



    }
}
