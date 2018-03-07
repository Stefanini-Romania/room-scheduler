using RSData.Models;
using RSRepository;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class EventService : IEventService
    {
        private IEventRepository eventRepository;
        private IAvailabilityRepository availabilityRepository;

        public EventService(IEventRepository eventRepository, IAvailabilityRepository availabilityRepository)
        {
            this.eventRepository = eventRepository;
            this.availabilityRepository = availabilityRepository;
        }

        public EventService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public double GetTimeSpan(DateTime start, DateTime end)
        {
            return (end - start).TotalMinutes;
        }

        public int GetAvailableTime(int userId, DateTime startDate)
        {
            var result = eventRepository.GetEventsByDay(startDate, userId);

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


        public bool CheckAvailability(DateTime startDate, DateTime endDate, int roomId)
        {
            var events = eventRepository.GetEventsByRoom(startDate.AddMinutes(-30), startDate.AddMinutes(30), roomId);

            foreach (Event ev in events)
            {
                if (ev.EventStatus != (int)EventStatusEnum.cancelled)
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
            }
            return true;
        }

        public bool HourCheck(DateTime startDate, DateTime endDate, int roomId)
        {
            var availabilities = availabilityRepository.GetAvailabilitiesByType(startDate, endDate, roomId);

            foreach (Availability ev in availabilities)

                if (startDate.TimeOfDay >= ev.StartDate.TimeOfDay && startDate.TimeOfDay < ev.EndDate.TimeOfDay)
                    return false;
            return true;
        }

        public bool IsAvailableTimeSpan(AddEventDto eventDto)
        {
            double eventTimeSpan = GetTimeSpan((DateTime)eventDto.StartDate, (DateTime)eventDto.EndDate);

            int availableTimeSpan = GetAvailableTime(eventDto.AttendeeId, (DateTime)eventDto.StartDate);

            return eventTimeSpan <= availableTimeSpan;
        }

        public bool IsGoodTimeSpan(AddEventDto eventDto)
        {
            double eventTimeSpan = GetTimeSpan((DateTime)eventDto.StartDate, (DateTime)eventDto.EndDate);

            if (eventTimeSpan != 30 && eventTimeSpan != 60)
            {
                return false;
            }
            return true;
        }

        public bool IsGoodStartTime(DateTime? d)
        {
            if (d.HasValue)
            {
                return d.Value.Hour >= 9 && d.Value.Hour <= 17 && d.Value.Second == 0 && (d.Value.Minute == 0 || d.Value.Minute == 30);
            }
            return false;
        }

        public bool IsGoodEndTime(DateTime? d)
        {
            if (d.HasValue)
            {
                return d.Value.Hour >= 9 && d.Value.Hour <= 17 && d.Value.Second == 0 && (d.Value.Minute == 0 || d.Value.Minute == 30) ||
                       d.Value.Hour == 18 && d.Value.Second == 0 && d.Value.Minute == 0;
            }
            return false;
        }

        public bool IsGoodDayOfWeek(DateTime? d)
        {
            if (d.HasValue)
            {
                return (d.Value.DayOfWeek >= DayOfWeek.Monday && d.Value.DayOfWeek <= DayOfWeek.Friday);
            }
            return false;
        }

        public bool IsInGoodRange(DateTime? d)
        {
            if ((d.Value.Month <= DateTime.Now.Month + 1) && (d.Value.Day >= 1 && d.Value.Day <= 31) && (d.Value.Year == DateTime.Now.Year))
                return true;

            if ((d.Value.Year == DateTime.Now.Year + 1) && (d.Value.Month == 01 && DateTime.Now.Month == 12))
                return true;
            return false;
        }

    }
}
