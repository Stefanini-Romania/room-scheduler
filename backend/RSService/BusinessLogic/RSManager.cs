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

        public RSManager(IAvailabiltyRepository _availabiltyRepository)
        {
            availabilityRepository = _availabiltyRepository;
        }


        public IEnumerable<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] hostId, int[] roomId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var allAvailabilities = availabilityRepository.GetAvailabilities();

            DateTime currentDay = startDate.Date;
 
            while (endDate.Date >= currentDay)
            {
                foreach (int room in roomId)
                {
                    foreach (int host in hostId)
                    {
                        var availabilities = allAvailabilities.Where(e => e.HostId == host)
                                                              .Where(e => e.RoomId == room)
                                                              .Where(e => e.DayOfWeek == (int)currentDay.DayOfWeek);
                        foreach (Availability entry in availabilities)
                        {
                            Event newEvent = new Event()
                            {
                                StartDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.StartHour.Hour, entry.StartHour.Minute, entry.StartHour.Second),
                                EndDate = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, entry.EndHour.Hour, entry.EndHour.Minute, entry.EndHour.Second),
                                EventType = "availability",
                                RoomId = room,
                                HostId = host,
                                EventStatus = (int)AvailabilityEnum.NotAvailable,
                            };
                            availabilityEvents.Add(newEvent);
                        }

                    }
                }
                currentDay = currentDay.AddDays(1);
            }

            return availabilityEvents;

        }
    }
}
