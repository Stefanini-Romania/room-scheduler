using RSData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IRSManager
    {
        IEnumerable<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] hostId, int[] roomId);
    }
}
