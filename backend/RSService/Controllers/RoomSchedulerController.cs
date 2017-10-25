using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Controllers
{
   // [Authorize]
    public class RoomSchedulerController : BaseController
    {
        private IEventRepository _eventRepository;
        private IDbTransaction _dbTransaction;

        public RoomSchedulerController(IEventRepository eventRepository, IDbTransaction dbTransaction)
        {
            _eventRepository = eventRepository;
            _dbTransaction = dbTransaction;
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

        [HttpGet("/event/list")]
        public IActionResult GetEvents(DateTime startDate, DateTime endDate)
        {
            var results = _eventRepository.GetEvents();
            if (results == null) return NotFound();

            return Ok(results);
        }

        [HttpGet("/event/list")]
        public IActionResult GetEvents(DateTime startDate, DateTime endDate, int roomId, int hostId)
        {
            var results = _eventRepository.GetEvents();
            if (results == null) return NotFound();

            return Ok(results);
        }

        [HttpPut("event/edit")]
        public IActionResult UpdateEvent(int id, [FromBody] EventViewModel model)
        {
            if (ModelState.IsValid)
            {

                var _event = _eventRepository.GetEvents().FirstOrDefault(c => c.Id == id);
                if (_event == null)
                {
                    return NotFound();
                }
                _event.StartDate = model.StartDate;
                _event.EndDate = model.EndDate;
                _event.EventType = model.EventType;
                _event.RoomId = model.RoomId;
                _event.Notes = model.Notes;
                _event.HostId = model.HostId;
                _event.AttendeeId = model.AttendeeId;
                _event.EventStatus = model.EventStatus;
                _event.DateCreated = DateTime.UtcNow;
                _dbTransaction.Commit();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



    }
}
