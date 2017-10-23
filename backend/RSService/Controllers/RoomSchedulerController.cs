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
    [Route("api/event")]
    public class RoomScheduler : BaseController
    {
        private IEventRepository _eventRepository;
        public RoomScheduler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
    
        [HttpPost]
        public void AddEvent([FromBody]Event value)
        {
            _eventRepository.AddEvent(value);
        }

    }
}
