using RSData.Models;
using System;
using RSRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class RSManager : IRSManager
    {
        private IAvailabilityRepository availabilityRepository;
        private IPenaltyRepository penaltyRepository;
        private IEventRepository eventRepository;
        private IUserRoleRepository userRoleRepository;
        private IUserRepository userRepository;
        private IDbOperation dbOperation;
        private IRoleRepository roleRepository;
        private IRoomRepository roomRepository;

        public RSManager(IAvailabilityRepository availabiltyRepository, IRoomRepository roomRepository, IEventRepository eventRepository, IPenaltyRepository penaltyRepository, IDbOperation dbOperation, IUserRoleRepository userRoleRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.availabilityRepository = availabiltyRepository;
            this.eventRepository = eventRepository;
            this.penaltyRepository = penaltyRepository;
            this.userRoleRepository = userRoleRepository;
            this.dbOperation = dbOperation;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.roomRepository = roomRepository;
        }


        public List<Event> CreateAvailabilityEvents(DateTime startDate, DateTime endDate, int?[] roomId, int?[] hostId)
        {
            List<Event> availabilityEvents = new List<Event>();

            var availabilities = availabilityRepository.GetAvailabilities(roomId, hostId);

            DateTime currentDay = startDate.Date;

            int fakeId = 1;
            while (endDate.Date >= currentDay)
            {
                availabilities = availabilities.Where(e => e.DayOfWeek == (int)currentDay.DayOfWeek).ToList();

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
                var dayAvailabilities = availabilities.Where(e => e.DayOfWeek == (int)currentDay.DayOfWeek).ToList();

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

//-------------------------------------------- METHODS FOR VALIDATION ----------------------------------------------------------------------

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

        public bool IsUniqueEmail(String email)
        {
            var user = userRepository.GetUserByEmail(email);

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsUniqueEmailEdit(String email, int userId)
        {
            var users = userRepository.GetUsersByEmail(email, userId);

            if (users.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool HourCheck(DateTime startDate, DateTime endDate, int roomId)
        {
            var availabilities = availabilityRepository.GetAvailabilitiesByType(startDate,endDate ,roomId);

            foreach (Availability ev in availabilities)
                                
                        if (startDate.TimeOfDay >= ev.StartDate.TimeOfDay && startDate.TimeOfDay < ev.EndDate.TimeOfDay)
                            return false;                   
            return true;
        }


        //Checks if the attendee has been marked as 'absent' three times in the current month and creates a new penalty entry in database.

        public void AddPenalty(DateTime startDate, int eventId, int attendeeId, int roomId)
        {
            var pastEvents = eventRepository.GetPastEventsByUser(startDate, attendeeId, roomId);

            var eventsCount = pastEvents.Count();

            if (eventsCount == 3)
            {
                penaltyRepository.AddPenalty(new Penalty()
                {
                    AttendeeId = attendeeId,
                    EventId = eventId,
                    Date = startDate,
                    RoomId = roomId
                });

                // Mark these 3 events as being part of a penalty to prevent future counting:

                foreach(Event pastEvent in pastEvents)
                {
                    pastEvent.EventStatus = (int)EventStatusEnum.absentChecked;
                }

                // Edit attendee's events for next 15 days for this room (Cancelled):

                var futureEvents = eventRepository.GetFutureEvents(startDate, attendeeId, roomId);

                if (futureEvents.Count() > 0)
                {
                    foreach (Event xEvent in futureEvents)
                    {
                        xEvent.EventStatus = (int)EventStatusEnum.cancelled;
                    }
                }
                dbOperation.Commit();
            }
        }

        public bool HasPenalty(int attendeeId, DateTime newDate, int roomId)
        {
            Penalty penalty = penaltyRepository.GetPenaltiesByUser(attendeeId)
                                    .FirstOrDefault(p => p.Date.AddDays(15) >= newDate);

            if (penalty != null)
            {
                return true;
            }

            return false;
        }

        public bool IsValidRole(List<int> userRole)
        {
            foreach (var roleId in userRole)
            {
                var role = roleRepository.GetRoleById(roleId);

                if (role == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsActiveUser(String email)
        {
            var activeUser = userRepository.GetUserByEmailAndActive(email);

            if (activeUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsUniqueRoom(String name, String location, int roomid)
        {
            var rooms = roomRepository.GetRoomByNameAndLocation(name, location,roomid);

            if (rooms == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
