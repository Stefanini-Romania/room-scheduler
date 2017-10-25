using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;

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
    
        [HttpPost("api/event/create")]
        public void AddEvent([FromBody]Event value)
        {
            _eventRepository.AddEvent(value);
            _dbTransaction.Commit();
        }
        
        [HttpGet("api/event/list")]
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
        [HttpGet("api/room/list")]
        public IActionResult GetRooms()
        {
            var results = _roomRepository.GetRooms();
            if (results == null) return NotFound();

            return Ok(results);
        }

    }
}
