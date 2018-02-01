using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IAvailabilityRepository
    {
        Availability GetAvailabilityById(int id);
        List<Availability> GetAvailabilities();
        List<Availability> GetAvailabilities(int?[] roomId, int?[] hostId);
        List<Availability> GetAvailabilities(int?[] roomId);
        List<Availability> GetAvailabilitiesByHost(int hostId);
        List<Availability> GetAvailabilitiesByRoom(DateTime startDate, DateTime endDate, int roomId);
        List<Availability> GetAvailabilitiesByHour(DateTime startDate, int roomId);
        List<Availability> GetAvailabilitiesByType(int availabilityType, DateTime startDate, DateTime endDate);
        List<Availability> GetAvailabilitiesByType(DateTime startDate, DateTime endDate, int roomId);
        void AddAvailability(Availability availability);
    }
   
}
