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
    public class RSManager
    {
        private IAvailabiltyRepository availabilityRepository;

        public RSManager(IAvailabiltyRepository _availabiltyRepository)
        {
            availabilityRepository = _availabiltyRepository;
        }

        public List<DayOfWeek> GetDaysOfWeek(DateTime start, DateTime end)
        {
            //maxim 7, dar pot fi mai putine
        }

        public int GetNumOfDays(DateTime start, DateTime end)
        {
            TimeSpan ts = (end - start);
            return  ts.Days;

        }


        public IEnumerable<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] hostId, int[] roomId, int availabilityType)
        {
            List<Event> availabilityEvents = new List<Event>();

            List<DayOfWeek> daysOfWeek = GetDaysOfWeek();

            var results = availabilityRepository.GetAvailabilities();
                              //.Where(e => e.DayOfWeek >= (int)startDate.DayOfWeek)
        
                              //.Where(e => hostId.Contains(e.HostId))
                              //.Where(e => roomId.Contains(e.RoomId))
                              //.Where(e => e.AvailabilityType == availabilityType);




            TimeSpan ss = new TimeSpan()
              
                
            
            
            foreach (Availability availability in results)
            {
                var newEvent = new Event()
                {
                    StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, availability.StartHour.Hour, availability.StartHour.Minute, availability.StartHour.Second),
                    EndDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, availability.EndHour.Hour, availability.EndHour.Minute, availability.EndHour.Second),
                    EventType = "availability",
                    RoomId = availability.RoomId,
                    HostId = availability.HostId,
                    EventStatus = availability.AvailabilityType
                };

                availabilityEvents.Add(newEvent);
            }

            int noDays = GetNumOfDays(startDate, endDate);


            DateTime currentDay = startDate.Date;
     
            while(endDate.Date >= currentDay)
            {
                //foreach room
                //foreach host
                //eventstart = currentDay + StartHour 
                //event end = cuurentDay + EndHour



                currentDay.AddDays(1);
            }





        }
    }
}
