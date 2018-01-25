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
        List<Availability> GetAvailabilitiesByRoom(DateTime startDate, DateTime endDate, int roomId);
        List<Availability> GetAvailabilitiesByHour(DateTime startDate, int roomId);
        List<Availability> GetAvailabilitiesByType(DateTime startDate, DateTime endDate, int roomId);
        List<Availability> GetAvailabilities(int availabilityType, int? roomId, int? hostId);
        void AddAvailability(Availability availability);
        void RemoveAvailabilities(List<Availability> availabilityList);
    }
   
}
