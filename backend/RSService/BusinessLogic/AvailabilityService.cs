using RSData.Models;
using RSRepository;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class AvailabilityService : IAvailabilityService
    {
        private IAvailabilityRepository availabilityRepository;

        public AvailabilityService(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }



        public List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int?[] roomId, int?[] hostId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var availabilities = availabilityRepository.GetAvailabilities(roomId, hostId);

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

            var availabilities = availabilityRepository.GetAvailabilities(roomId);

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
            var availabilities = availabilityRepository.GetOverlapedAvailabilities(newAvailability.StartDate, newAvailability.EndDate, newAvailability.RoomId);

            if (!availabilities.Any())
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

        public bool IsGoodStartTime(AvailabilityDto availabilityDto)
        {
            return availabilityDto.StartDate.Hour >= 9 && availabilityDto.StartDate.Hour <= 17 && availabilityDto.StartDate.Second == 0 && 
                   (availabilityDto.StartDate.Minute == 0 || availabilityDto.StartDate.Minute == 30);
        }

        public bool IsGoodStartTime(AvailabilityExceptionDto availabilityDto)
        {
            return availabilityDto.StartDate.Hour >= 9 && availabilityDto.StartDate.Hour <= 17 && availabilityDto.StartDate.Second == 0 &&
                   (availabilityDto.StartDate.Minute == 0 || availabilityDto.StartDate.Minute == 30);
        }

        public bool IsGoodEndTime(AvailabilityDto availabilityDto)
        {
            return availabilityDto.EndDate.Hour >= 9 && availabilityDto.EndDate.Hour <= 17 && availabilityDto.EndDate.Second == 0 && 
                   (availabilityDto.EndDate.Minute == 0 || availabilityDto.EndDate.Minute == 30) ||
                   availabilityDto.EndDate.Hour == 18 && availabilityDto.EndDate.Second == 0 && availabilityDto.EndDate.Minute == 0;
        }

        public bool IsGoodEndTime(AvailabilityExceptionDto availabilityDto)
        {
            return availabilityDto.EndDate.Hour >= 9 && availabilityDto.EndDate.Hour <= 17 && availabilityDto.EndDate.Second == 0 &&
                   (availabilityDto.EndDate.Minute == 0 || availabilityDto.EndDate.Minute == 30) ||
                   availabilityDto.EndDate.Hour == 18 && availabilityDto.EndDate.Second == 0 && availabilityDto.EndDate.Minute == 0;
        }

        public bool IsGoodStartDate(AvailabilityExceptionDto availabilityDto)
        {
            return availabilityDto.StartDate.Date == availabilityDto.EndDate.Date;
        }

        public bool IsGoodEndDate(AvailabilityExceptionDto availabilityDto)
        {
            return availabilityDto.EndDate.Date == availabilityDto.StartDate.Date;
        }

        public bool ValidDays(AddAvailabilityDto availabilityDto)
        {
            if (availabilityDto.DaysOfWeek == null)
            {
                return false;
            }
            if (availabilityDto.DaysOfWeek.Length > 5)
            {
                return false;
            }

            foreach (var day in availabilityDto.DaysOfWeek)
            {
                if (day != 1 && day != 2 && day != 3 && day != 4 && day != 5)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ValidOccurrence(AvailabilityDto availabilityDto)
        {
            if (availabilityDto.Occurrence != 1 && availabilityDto.Occurrence != 2 && availabilityDto.Occurrence != 3 && availabilityDto.Occurrence != 4)
            {
                return false;
            }

            return true;
        }

        public bool ValidStatus(EditAvailabilityDto availabilityDto)
        {
            if (availabilityDto.Status != 0 && availabilityDto.Status != 1)
            {
                return false;
            }
            return true;
        }

    }
}
