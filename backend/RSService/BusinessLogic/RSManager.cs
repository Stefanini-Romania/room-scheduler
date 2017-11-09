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
        private IPenaltyRepository penaltyRepository;
        private IEventRepository eventRepository;

        public RSManager(IAvailabiltyRepository _availabiltyRepository, IEventRepository _eventRepository, IPenaltyRepository _penaltyRepository)
        {
            availabilityRepository = _availabiltyRepository;
            eventRepository = _eventRepository;
            penaltyRepository = _penaltyRepository;
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

            return availabilityEvents;

        }

        public IEnumerable<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int[] roomId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var availabilities = availabilityRepository.GetAvailabilities(roomId);

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

            return availabilityEvents;

        }

//-------------------------------------------- METHODS FOR VALIDATION ----------------------------------------------------------------------

        public double GetTimeSpan(DateTime start, DateTime end)
        {
            return (end - start).TotalMinutes;
        }

        public int GetAvailableTime(int userId, DateTime startDate)
        {
            var result = eventRepository.GetEvents()
                            .Where(e => e.AttendeeId == userId)
                            .Where(e => e.StartDate.Date == startDate.Date);
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
            else return 60;
        }

        public bool CanCancel(DateTime startDate, DateTime endDate, int roomId, int attendee)
        {
            var aux = eventRepository.GetEvents().Where(e => e.StartDate == startDate).Where(e => e.EndDate == endDate)
                                     .Where(e => e.RoomId == roomId).Where(e => e.AttendeeId == attendee);
            return aux.Count() != 0;
        }

        public bool CheckAvailability(DateTime startDate, DateTime endDate, int roomId)
        {
            var events = eventRepository.GetEventsByRoom(startDate.AddMinutes(-30), startDate.AddMinutes(30), roomId);

            foreach (Event ev in events)
            {
                if (startDate == ev.StartDate || endDate == ev.EndDate)
                {
                    return false;
                }

                if (startDate > ev.StartDate)
                {
                    if (startDate < ev.EndDate)
                    {
                        return false;
                    }
                }

                if (startDate < ev.StartDate)
                {
                    if (endDate > ev.StartDate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }






        //public bool IsPenalizedAttendee(int attendeeId)
        //{

        //    return false;
        //}

        
        //Checks if the attendee has been marked as 'absent' three times in the current month and creates a new penalty entry in database.
            
        //public void CheckPenalty(DateTime startDate, int eventId, int attendeeId)
        //{
        //    var eventsCount = eventRepository.GetEvents().Where(ev => ev.AttendeeId == attendeeId)
        //                                .Where(ev => ev.StartDate.Month == DateTime.Now.Month) // TO DO: current month or last 30 days ?
        //                                .Where(ev => ev.EventStatus == (int)EventStatusEnum.absent).Count();
        //    if (eventsCount == 3)
        //    {
        //        penaltyRepository.AddPenalty(new Penalty()
        //        {
        //            AttendeeId = attendeeId,
        //            EventId = eventId,
        //            Date = startDate
        //        });
        //    }
        //}

        public bool HasPenalty(int attendeeId, DateTime newDate)
        {
            Penalty penalty = penaltyRepository.GetPenaltiesByUser(attendeeId)
                                    .SingleOrDefault(p => p.Date.AddDays(15) >= newDate);

            if (penalty != null)
            {
                return true;
            }

            return false;
        }

    }
}
