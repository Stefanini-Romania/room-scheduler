using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.BusinessLogic;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RSService.DTO;
using MimeKit;
using MailKit.Net.Smtp;

namespace RSService.Controllers
{
    public class RoomSchedulerController : BaseController
    {
        private IEventRepository eventRepository;
        private IRoomRepository roomRepository;
        private IAvailabilityRepository availabilityRepository;
        private IUserRepository userRepository;
        private IRSManager rsManager;

        public RoomSchedulerController(IRSManager rsManager)
        {
            this.roomRepository = new RoomRepository(Context);
            this.availabilityRepository = new AvailabilityRepository(Context);
            this.eventRepository = new EventRepository(Context);
            this.userRepository = new UserRepository(Context);
            this.availabilityRepository = new AvailabilityRepository(Context);
            this.rsManager = rsManager;
        }

        [HttpPost("/event/create")]
        [Authorize]
        public IActionResult AddEvent([FromBody]AddEventDto model)
        {
            if (!ModelState.IsValid)
                return ValidationError(GeneralMessages.Event);

            var schedulerIdentity = SchedulerIdentity.Current(HttpContext);
            var currentAttendeeId = schedulerIdentity.UserId;
            var currentAttendeeEmail = schedulerIdentity.Email;

            var attendee = userRepository.GetUserById(currentAttendeeId);
            if (attendee.IsActive != true)
            {
                return ValidationError(EventMessages.InactiveUser);
            }

            var newEvent = Mapper.Map<Event>(model);
            newEvent.DateCreated = DateTime.UtcNow;
            newEvent.AttendeeId = currentAttendeeId;

            var startDateText = model.StartDate.ToString();
            DateTime startDate = new DateTime();
            DateTime.TryParse(startDateText, out startDate);

            var avList = availabilityRepository.GetAvailabilitiesByHour(startDate, model.RoomId);
            var first = avList.FirstOrDefault();
            if (first != null)
            {
                newEvent.HostId = first.HostId;
            }
            else
            {
                newEvent.HostId = null;  // no host (not a massage room)
            }

            eventRepository.AddEvent(newEvent);
            Context.SaveChanges();

            var room = roomRepository.GetRoomById(newEvent.RoomId);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RoomSchedulerStefanini", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("User", currentAttendeeEmail));
            message.Subject = "You have an appoitment";
            message.Body = new TextPart("html")
            {
                Text = " You have a new appoitment. <br> " + "Date: " + newEvent.StartDate.Day + "/" + newEvent.StartDate.Month + "/" + newEvent.StartDate.Year + "<br>"
                + " Hour: " + newEvent.StartDate.TimeOfDay + " to " + newEvent.EndDate.TimeOfDay + "<br>"
                + " Room Name: " + room.Name + "<br>"
                + " Room Location: " + room.Location
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("roomchedulerStefanini@gmail.com", "admin123456");

                client.Send(message);

                client.Disconnect(true);
            }

            return Ok(new
            {
                Id = newEvent.Id,
                StartDate = newEvent.StartDate,
                EndDate = newEvent.EndDate
            });
         
        }

        [HttpGet("/event/listall")]
        public IActionResult GetEvents()
        {
            var results = eventRepository.GetEvents();
            if (results == null) return NotFound();

            return Ok(results);
        }

        [HttpGet("/event/list")]
        public IActionResult GetEventsByHosts(DateTime startDate, DateTime endDate, int?[] roomId, int?[] hostId)
        {
            if (!hostId.Any())
                return GetEvents(startDate, endDate, roomId);
            
            var results = eventRepository.GetEvents(startDate, endDate, roomId, hostId);

            int?[] avRoomId = roomId;
         
            var availabilityEvents = rsManager.CreateAvailabilityEvents(startDate, endDate, roomId, hostId);

            results = results.Concat(availabilityEvents).ToList();

            if (results == null)
                return NotFound();

            List<EventDto> events = new List<EventDto>();

            foreach (var ev in results)
            {
                events.Add(new EventDto()
                {
                    Id = ev.Id,
                    StartDate = ev.StartDate,
                    EndDate = ev.EndDate,
                    EventType = ev.EventType,
                    RoomId = ev.RoomId,
                    Notes = ev.Notes,
                    HostId = ev.HostId,
                    AttendeeId = ev.AttendeeId,
                    EventStatus = ev.EventStatus,
                    Host = ev.Host.FirstName + " " + ev.Host.LastName,
                    DateCreated = ev.DateCreated
                });
            }

            return Ok(events);
        }
      
        public IActionResult GetEvents(DateTime startDate, DateTime endDate, int?[] roomId)
        {
            var results = eventRepository.GetEvents(startDate, endDate, roomId);

            var availabilityEvents = rsManager.CreateAvailabilityEvents(startDate, endDate, roomId);

            results = results.Concat(availabilityEvents).ToList();

            if (results == null)
                return NotFound();

            List<EventDto> events = new List<EventDto>();

            foreach(var ev in results)
            {
                events.Add(new EventDto()
                {
                    Id = ev.Id,
                    StartDate = ev.StartDate,
                    EndDate = ev.EndDate,
                    EventType = ev.EventType,
                    RoomId = ev.RoomId,
                    Notes = ev.Notes,
                    HostId = ev.HostId,
                    AttendeeId = ev.AttendeeId,
                    EventStatus = ev.EventStatus,
                    Host = (ev.Host !=null) ?  ev.Host.FirstName +" "+ ev.Host.LastName : null,
                    DateCreated = ev.DateCreated
                });
            }

            return Ok(events);
        }

        [HttpPut("/event/edit/{id}")]
        [Authorize]
        public IActionResult UpdateEvent(int id, [FromBody] EditEventDto model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.EventEdit);
            }

            var schedulerIdentity = SchedulerIdentity.Current(HttpContext);
            var currentUserId = schedulerIdentity.UserId;


            var _model = Mapper.Map<Event>(model);

            var _event = eventRepository.GetEventById(id);

            if (_event == null)
            {
                return NotFound();
            }

            // If the event was not created by current user OR current user is not the host for this event
            if (currentUserId != _event.AttendeeId && currentUserId != _event.HostId)
            {
                return ValidationError(EventMessages.CancellationRight);
            }

            if (currentUserId == _event.HostId)
            {
                if (model.EventStatus != (int)EventStatusEnum.present && model.EventStatus != (int)EventStatusEnum.absent)
                {
                    return ValidationError(EventMessages.WrongEventStatus);
                }
            }

            _event.Notes = _model.Notes;
            _event.EventStatus = _model.EventStatus;
            _event.DateCreated = DateTime.UtcNow;

            Context.SaveChanges();

            if (_event.EventStatus == (int)EventStatusEnum.absent)
                rsManager.AddPenalty(_event.StartDate, _event.Id, _event.AttendeeId, _event.RoomId);    // works only if host updates the events daily

            return Ok(new
            {
                Id = _event.Id,
                StartDate = _event.StartDate,
                EndDate = _event.EndDate
            });
          
        }

       
    }
}
