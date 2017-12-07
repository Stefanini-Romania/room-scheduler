using Microsoft.EntityFrameworkCore;
using RoomScheduler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IAvailabiltyRepository
    {
        Availability GetAvailabilityById(int id);
        List<Availability> GetAvailabilities();
        List<Availability> GetAvailabilities(int[] roomId, int[] hostId);
        List<Availability> GetAvailabilities(int[] roomId);
        List<Availability> GetAvailabilitiesByRoom(DateTime startDate, DateTime endDate, int roomId);
        List<Availability> GetAvailabilitiesByHour(DateTime startDate, int roomId);
        List<Availability> GetAvailabilitiesByType(DateTime startDate, DateTime endDate, int roomId);
    }
   
}
