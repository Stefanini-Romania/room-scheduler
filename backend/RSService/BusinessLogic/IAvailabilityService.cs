using RSData.Models;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IAvailabilityService
    {
        List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int?[] roomId, int?[] hostId);

        List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int?[] roomId);

        bool IsOverlapedAvailability(Availability availability);

        bool IsGoodStartTime(AddAvailabilityDto availabilityDto);

        bool IsGoodEndTime(AddAvailabilityDto availabilityDto);

    }
}
