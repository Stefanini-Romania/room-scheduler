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
        private IAvailabilityRepository _availabilityService;

        public AvailabilityService(IAvailabilityRepository availabilityService)
        {
            _availabilityService = availabilityService;
        }



        public List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int?[] roomId, int?[] hostId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var availabilities = _availabilityService.GetAvailabilities(roomId, hostId);

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

            var availabilities = _availabilityService.GetAvailabilities(roomId);

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
    }
}
