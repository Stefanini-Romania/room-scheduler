using RSData.Models;
using RSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class AvailabilityService : IAvailabilityService
    {
        private IAvailabilityRepository _availabilityRepository;

        public AvailabilityService(IAvailabilityRepository availabilityService)
        {
            _availabilityRepository = availabilityService;
        }



        public List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int?[] roomId, int?[] hostId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var availabilities = _availabilityRepository.GetAvailabilities(roomId, hostId);

            DateTime currentDay = startDate.Date;

            int fakeId = 1;
            while (endDate.Date >= currentDay)
            {
                availabilities = availabilities.Where(e => e.StartDate.DayOfWeek == currentDay.DayOfWeek).ToList();

                foreach (Availability entry in availabilities)
                {
                    Event newEvent = new Event()
                    {
                        Id = -fakeId++,
                        StartDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.StartDate.Hour, entry.StartDate.Minute, entry.StartDate.Second),
                        EndDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.EndDate.Hour, entry.EndDate.Minute, entry.EndDate.Second),
                        EventType = (int)EventTypeEnum.availability,
                        RoomId = (int)entry.RoomId,
                        HostId = entry.HostId,
                        EventStatus = entry.AvailabilityType,
                        DateCreated = DateTime.UtcNow,
                        Host = entry.Host
                    };
                    availabilityEvents.Add(newEvent);
                }

                currentDay = currentDay.AddDays(1);
            }

            return availabilityEvents;

        }

        public List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int?[] roomId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var availabilities = _availabilityRepository.GetAvailabilities(roomId);

            DateTime currentDay = startDate.Date;

            int fakeId = 1;
            while (endDate.Date >= currentDay)
            {
                var dayAvailabilities = availabilities.Where(e => e.StartDate.DayOfWeek == currentDay.DayOfWeek).ToList();

                foreach (Availability entry in dayAvailabilities)
                {
                    Event newEvent = new Event()
                    {
                        Id = -fakeId++,
                        StartDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.StartDate.Hour, entry.StartDate.Minute, entry.StartDate.Second),
                        EndDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.EndDate.Hour, entry.EndDate.Minute, entry.EndDate.Second),
                        EventType = (int)EventTypeEnum.availability,
                        RoomId = (int)entry.RoomId,
                        HostId = entry.HostId,
                        EventStatus = entry.AvailabilityType,
                        DateCreated = DateTime.UtcNow,
                        Host = entry.Host
                    };
                    availabilityEvents.Add(newEvent);
                }

                currentDay = currentDay.AddDays(1);
            }

            return availabilityEvents;
        }

        public bool IsOverlapedAvailability(Availability newAvailability)
        {
            var availabilities = _availabilityRepository.GetOverlapedAvailabilities(newAvailability.StartDate, newAvailability.EndDate, newAvailability.RoomId);

            if (availabilities == null)
            {
                return false;
            }

            if (newAvailability.Occurrence == 1)
            {
                return true;
            }

            foreach(var av in availabilities)
            {
                if (av.Occurrence == 1)
                {
                    return true;
                }

                DateTimeOffset startDate = av.StartDate;
                DateTimeOffset auxStartDate = newAvailability.StartDate.Date.Add(av.StartDate.TimeOfDay);

                long diff = auxStartDate.ToUnixTimeSeconds() - startDate.ToUnixTimeSeconds();
                
                double occurrenceRaport = (double)diff / (7 * av.Occurrence * 24 * 60 * 60);

                if (occurrenceRaport % 1 == 0)
                {
                    return true;
                }

                if (av.Occurrence % newAvailability.Occurrence != 0 && newAvailability.Occurrence % av.Occurrence != 0)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
