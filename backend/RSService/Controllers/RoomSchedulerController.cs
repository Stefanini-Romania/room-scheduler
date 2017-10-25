using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.ViewModels;
using System;
using System.Linq;

namespace RSService.Controllers
{
    // [Authorize]
    public class RoomSchedulerController : BaseController
    {
        private IEventRepository _eventRepository;
        private IDbTransaction _dbTransaction;
        private IRoomRepository _roomRepository;

        public RoomSchedulerController(IEventRepository eventRepository, IDbTransaction dbTransaction, IRoomRepository roomRepository)
        {
            _eventRepository = eventRepository;
            _dbTransaction = dbTransaction;
            _roomRepository = roomRepository;

        }
    
        [HttpPost("/event/create")]
        public void AddEvent([FromBody]EventViewModel model)
        {
            var newEvent = Mapper.Map<Event>(model);
            newEvent.DateCreated = DateTime.UtcNow;

            _eventRepository.AddEvent(newEvent);
            _dbTransaction.Commit();
        }
        
        [HttpGet("/event/list")]
        public IActionResult GetEvents()
        {
            var results = _eventRepository.GetEvents();
            if (results == null) return NotFound();

            return Ok(results);
        }

        //[HttpGet("/event/list")]
        public IActionResult GetEvents(DateTime startDate, DateTime endDate)
        {
            var results = _eventRepository.GetEvents();
            if (results == null) return NotFound();

            return Ok(results);
        }

        //[HttpGet("/event/list")]
        public IActionResult GetEvents(DateTime startDate, DateTime endDate, int roomId, int hostId)
        {
            var results = _eventRepository.GetEvents();
            if (results == null) return NotFound();

            return Ok(results);
        }

        [HttpPut("/event/edit/{id}")]
        public IActionResult UpdateEvent(int id, [FromBody] EventViewModel model)
        {
            if (ModelState.IsValid)
            {

                var _model = Mapper.Map<Event>(model);

                var _event = _eventRepository.GetEvents().FirstOrDefault(c => c.Id == id);
                if (_event == null)
                {
                    return NotFound();
                }

                _event.StartDate = _model.StartDate;
                _event.EndDate = _model.EndDate;
                _event.EventType = _model.EventType;
                _event.RoomId = _model.RoomId;
                _event.Notes = _model.Notes;
                _event.HostId = _model.HostId;
                _event.AttendeeId = _model.AttendeeId;
                _event.EventStatus = _model.EventStatus;
                _event.DateCreated = DateTime.UtcNow;
                _dbTransaction.Commit();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
       
        [HttpGet("/room/list")]
        public IActionResult GetRooms()
        {
            var results = _roomRepository.GetRooms();
            if (results == null) return NotFound();

            return Ok(results);
        }

    }
}
