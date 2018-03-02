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

        bool IsGoodStartTime(AvailabilityDto availabilityDto);

        bool IsGoodStartTime(AvailabilityExceptionDto availabilityDto);

        bool IsGoodEndTime(AvailabilityDto availabilityDto);

        bool IsGoodEndTime(AvailabilityExceptionDto availabilityDto);

        bool ValidDays(AddAvailabilityDto availabilityDto);

        bool ValidOccurrence(AvailabilityDto availabilityDto);

        bool ValidStatus(EditAvailabilityDto availabilityDto);

    }
}
