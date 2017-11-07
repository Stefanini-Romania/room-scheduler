using RSData.Models;
using System;
using RSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomScheduler.Models;

namespace RSService.BusinessLogic
{
    public class RSManager : IRSManager
    {
        private IAvailabiltyRepository availabilityRepository;
        private IEventRepository eventRepository;

        public RSManager(IAvailabiltyRepository _availabiltyRepository, IEventRepository _eventRepository)
        {
            availabilityRepository = _availabiltyRepository;
            eventRepository = _eventRepository;
        }


        public IEnumerable<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] roomId, int[] hostId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var availabilities = availabilityRepository.GetAvailabilities(roomId, hostId);

            DateTime currentDay = startDate.Date;

            while (endDate.Date >= currentDay)
            {
                availabilities = availabilities.Where(e => e.DayOfWeek == (int)currentDay.DayOfWeek);

                foreach (Availability entry in availabilities)
                {
                    Event newEvent = new Event()
                    {
                        StartDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.StartHour.Hour, entry.StartHour.Minute, entry.StartHour.Second),
                        EndDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.EndHour.Hour, entry.EndHour.Minute, entry.EndHour.Second),
                        EventType = 1,
                        RoomId = entry.RoomId,
                        HostId = entry.HostId,
                        EventStatus = (int)AvailabilityEnum.NotAvailable,
                        DateCreated = DateTime.UtcNow,
                    };
                    availabilityEvents.Add(newEvent);
                }

                currentDay = currentDay.AddDays(1);
            }


            //while (endDate.Date >= currentDay)
            //{
            //    foreach (int room in roomId)
            //    {
            //        foreach (int host in hostId)
            //        {
            //            //var availabilities = allAvailabilities.Where(e => e.DayOfWeek == (int)currentDay.DayOfWeek)
            //            //                                      .Where(e => e.RoomId == room)
            //            //                                      .Where(e => e.HostId == host);

            //            foreach (Availability entry in availabilities)
            //            {
            //                Event newEvent = new Event()
            //                {
            //                    StartDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.StartHour.Hour, entry.StartHour.Minute, entry.StartHour.Second),
            //                    EndDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.EndHour.Hour, entry.EndHour.Minute, entry.EndHour.Second),
            //                    EventType = 1,
            //                    RoomId = room,
            //                    HostId = host,
            //                    EventStatus = (int)AvailabilityEnum.NotAvailable,
            //                    DateCreated = DateTime.UtcNow,
            //                };
            //                availabilityEvents.Add(newEvent);
            //            }

            //        }
            //    }
            //    currentDay = currentDay.AddDays(1);
            //}

            return availabilityEvents;

        }

        public int GetTimeSpan(DateTime start, DateTime end)
        {
                return (end - start).Minutes; 
        }

        public int GetAvailableTime(int userId, DateTime startDate)
        {
            var result = eventRepository.GetEvents()
                            .Where(e => e.AttendeeId == userId)
                            .Where(e => e.StartDate == startDate);
            if (result.Count() >= 2)
            {
                return 0;
            }
            else if (result.Count() == 1)
            {
                if (GetTimeSpan(result.First().StartDate, result.First().EndDate) == 30)
                {
                    return 30;
                }
                else
                {
                    return 0; 
                }
            }
            return 60 ;
        }

        public bool CanCancel(DateTime startDate, DateTime endDate, int roomId, int attendee)
        {
           var aux = eventRepository.GetEvents().Where(e => e.StartDate == startDate).Where(e => e.EndDate == endDate)
                                    .Where(e => e.RoomId == roomId).Where(e => e.AttendeeId == attendee);
           return aux.Count() != 0;
        }

        public bool checkAvailability()
        {
            return true;
        }





    }
}
