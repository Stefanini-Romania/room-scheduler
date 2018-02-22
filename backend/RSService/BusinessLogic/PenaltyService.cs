using RSData.Models;
using RSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class PenaltyService : IPenaltyService
    {
        private IEventRepository eventRepository;
        private IPenaltyRepository penaltyRepository;
        
        public PenaltyService(IEventRepository eventRepository, IPenaltyRepository penaltyRepository)
        {
            this.eventRepository = eventRepository;
            this.penaltyRepository = penaltyRepository;
        }


        //Checks if the attendee has been marked as 'absent' three times in the current month and creates a new penalty entry in database.

        public void AddPenalty(DateTime startDate, int eventId, int attendeeId, int roomId)
        {
            var pastEvents = eventRepository.GetPastEventsByUser(startDate, attendeeId, roomId);  //Last 30 days

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

                foreach (Event pastEvent in pastEvents)
                {
                    pastEvent.EventStatus = (int)EventStatusEnum.absentChecked;
                }

                // Edit attendee's events for next 15 days for this room (Cancelled):

                var futureEvents = eventRepository.GetFutureEvents(startDate.AddDays(15), attendeeId, roomId);

                if (futureEvents.Count() > 0)
                {
                    foreach (Event xEvent in futureEvents)
                    {
                        xEvent.EventStatus = (int)EventStatusEnum.cancelled;
                    }
                }
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


    }
}
