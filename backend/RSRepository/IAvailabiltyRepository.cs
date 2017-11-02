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
        IEnumerable<Availability> GetAvailabilities();
        IEnumerable<Availability> GetAvailabilities(int[] roomId, int[] hostId);
    }
   
}
