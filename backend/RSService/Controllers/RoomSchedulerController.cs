﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.BusinessLogic;
using RSService.Validation;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RSService.Controllers
{
    public class RoomSchedulerController : BaseController
    {
        private IEventRepository eventRepository;
        private IRoomRepository roomRepository;
        private IAvailabiltyRepository availabilityRepository;
        private IUserRepository userRepository;
        private IRSManager rsManager;

        public RoomSchedulerController(IRSManager rsManager)
        {
            this.roomRepository = new RoomRepository(Context);
            this.availabilityRepository = new AvailabilityRepository(Context);
            this.eventRepository = new EventRepository(Context);
            this.userRepository = new UserRepository(Context);
            this.rsManager = rsManager;
        }

        [HttpPost("/event/create")]
        [Authorize]
        public IActionResult AddEvent([FromServices] FluentValidation.IValidator<EventViewModel> validator, [FromBody]EventViewModel model)
        {
            var userName = HttpContext.User.Identity.Name;
            var currentAttendeeId = userRepository.GetUsers().Single(u => u.Name == userName).Id;

            if (!ModelState.IsValid)
                return ValidationError(GeneralMessages.Event);

            var newEvent = Mapper.Map<Event>(model);
            newEvent.DateCreated = DateTime.UtcNow;
            newEvent.AttendeeId = currentAttendeeId;

            eventRepository.AddEvent(newEvent);
            Context.SaveChanges();

            // TODO: return DTO object
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
        public IActionResult GetEventsByHosts(DateTime startDate, DateTime endDate, int[] roomId, int[] hostId)
        {
            if (!hostId.Any())
                return GetEvents(startDate, endDate, roomId);

            var results = eventRepository.GetEvents(startDate, endDate, roomId, hostId);

            var availabilityEvents = rsManager.CreateAvailabilityEvents(startDate, endDate, roomId, hostId);

            results = results.Concat(availabilityEvents).ToList();

            return Ok(results);
        }

        public IActionResult GetEvents(DateTime startDate, DateTime endDate, int[] roomId)
        {
            var results = eventRepository.GetEvents(startDate, endDate, roomId);

            var availabilityEvents = rsManager.CreateAvailabilityEvents(startDate, endDate, roomId);

            results = results.Concat(availabilityEvents).ToList();

            return Ok(results);
        }

        [HttpPut("/event/edit/{id}")]
        [Authorize]
        public IActionResult UpdateEvent(int id, [FromBody] EditViewModel model)
        {
            var userName = HttpContext.User.Identity.Name;
            var currentAttendeeId = userRepository.GetUsers().Single(u => u.Name == userName).Id;

            if (ModelState.IsValid)
            {
                var _model = Mapper.Map<Event>(model);

                var _event = eventRepository.GetEvents().FirstOrDefault(c => c.Id == id);
                if (_event == null)
                {
                    return NotFound();
                }

                _event.Notes = _model.Notes;
                _event.EventStatus = _model.EventStatus;
                _event.DateCreated = DateTime.UtcNow;

                Context.SaveChanges();

                if (_event.EventStatus == (int)EventStatusEnum.absent)
                    rsManager.CheckPenalty(_event.StartDate, _event.Id, _event.AttendeeId, _event.RoomId);

                return Ok(new
                {
                    Id = _event.Id,
                    StartDate = _event.StartDate,
                    EndDate = _event.EndDate
                });
            }
            else
            {
                return ValidationError(GeneralMessages.Event);
            }
        }

        [HttpGet("/room/list")]
        public IActionResult GetRooms()
        {
            var results = roomRepository.GetRooms();
            if (results == null) return NotFound();

            return Ok(results);
        }
    }
}
