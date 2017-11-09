using System;
using System.Collections.Generic;
using System.Text;
using RoomScheduler.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RSRepository
{
    public class AvailabilityRepository : IAvailabiltyRepository
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
            return availabilities.SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Availability> GetAvailabilities()
        {
            return availabilities.AsEnumerable();
        }

        public IEnumerable<Availability> GetAvailabilities(int[] roomId, int[] hostId)
        {
            return availabilities.Where(e => roomId.Contains(e.RoomId))
                                 .Where(e => hostId.Contains(e.HostId))
                                 .AsEnumerable();
        }

        public IEnumerable<Availability> GetAvailabilities(int[] roomId)
        {
            return availabilities.Where(e => roomId.Contains(e.RoomId))
                                 .AsEnumerable();
        }


    }
}
