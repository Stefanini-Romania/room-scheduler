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
            if (rooms == null)
                return NotFound();

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

    }
}
