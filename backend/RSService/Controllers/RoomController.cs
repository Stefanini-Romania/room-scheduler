using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.DTO;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Controllers
{
    public class RoomController : BaseController
    {
        private IRoomRepository roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }


        [HttpGet("/room/list")]
        public IActionResult GetRooms()
        {
            var rooms = roomRepository.GetRooms();
            if (rooms == null) return NotFound();

            List<RoomDTO> roomList = new List<RoomDTO>();

            foreach (var it in rooms)
            {
                roomList.Add(new RoomDTO()
                {
                    Name = it.Name,
                    Location = it.Location
                });
            }

            return Ok(roomList);
        }

        [HttpPost("/room/add")]
        public IActionResult AddRoom([FromBody]RoomDTO model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Room);
            }

            if (model.Name == null || model.Location == null)
            {
                return BadRequest();
            }

            Room newRoom = new Room()
            {
                Name = model.Name,
                Location = model.Location
            };

            roomRepository.AddRoom(newRoom);

            Context.SaveChanges();
            return Ok(model);
        }

        //[HttpPut("/room/edit/{id}")]
        //public IActionResult UpdateRoom(int id, [FromBody] RoomDTO model)
        //{
        //    var userName = HttpContext.User.Identity.Name;
        //    var currentAttendeeId = userRepository.GetUsers().Single(u => u.Name == userName).Id;

        //    if (ModelState.IsValid)
        //    {
        //        var _model = Mapper.Map<Event>(model);

        //        var _event = eventRepository.GetEvents().FirstOrDefault(c => c.Id == id);
        //        if (_event == null)
        //        {
        //            return NotFound();
        //        }

        //        _event.Notes = _model.Notes;
        //        _event.EventStatus = _model.EventStatus;
        //        _event.DateCreated = DateTime.UtcNow;

        //        Context.SaveChanges();

        //        if (_event.EventStatus == (int)EventStatusEnum.absent)
        //            rsManager.CheckPenalty(_event.StartDate, _event.Id, _event.AttendeeId, _event.RoomId);

        //        return Ok(new
        //        {
        //            Id = _event.Id,
        //            StartDate = _event.StartDate,
        //            EndDate = _event.EndDate
        //        });
        //    }
        //    else
        //    {
        //        return ValidationError(GeneralMessages.Event);
        //    }
        //}


    }
}
