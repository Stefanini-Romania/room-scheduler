using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
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
    
        [HttpPost("api/addevent")]
        public void AddEvent([FromBody]Event value)
        {
            _eventRepository.AddEvent(value);
            _dbTransaction.Commit();
        }
        
        [HttpGet("api/events")]
        public IActionResult GetEvents()
        {
            var results = _eventRepository.GetEvents();
            if (results == null) return NotFound();

            return Ok(results);
        }

        [HttpDelete("api/deleteevent/{id}")]
        public void DeleteEvent(int id)
        {
            _eventRepository.DeleteEvent(id);
            _dbTransaction.Commit();
        }
    }
}
