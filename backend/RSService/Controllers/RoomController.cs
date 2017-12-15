using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.DTO;
using RSService.Validation;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Controllers
{
    public class RoomController : BaseController
    {
        private IRoomRepository roomRepository;
        private IUserRepository userRepository;

        public RoomController()
        {
            this.roomRepository = new RoomRepository(Context);
            this.userRepository = new UserRepository(Context);
        }

        [HttpGet("/room/list")]
        public IActionResult GetRooms()
        {

            var currentUserId = 0;
            var userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                currentUserId = userRepository.GetUserByUsername(userName).Id;
            }
            var currentUser = userRepository.GetUserById(currentUserId);

            var rooms = roomRepository.GetRoomsByStatus(true);

            //IF ADMIN:
            if (currentUser != null)
            {
                if (currentUser.UserRole.Select(li => li.RoleId).Contains((int)UserRoleEnum.admin))
                {
                    rooms = roomRepository.GetRooms();
                }
            }

            if (rooms == null) return NotFound();

            List<RoomDto> roomList = new List<RoomDto>();

            foreach (var it in rooms)
            {
                roomList.Add(new RoomDto()
                {
                    Id = it.Id,
                    Name = it.Name,
                    Location = it.Location,
                    IsActive = it.IsActive
                });
            }

            return Ok(roomList);
        }

        [HttpPost("/room/add")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult AddRoom([FromBody]EditRoomViewModel model) 
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
                Location = model.Location,
                IsActive = true
            };

            roomRepository.AddRoom(newRoom);

            Context.SaveChanges();
            return Ok(model);
        }

        [HttpPut("/room/edit/{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult UpdateRoom(int id, [FromBody] EditRoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Room);
            }
            else
            {

                var room = roomRepository.GetRoomById(id);
                if (room == null)
                {
                    return NotFound();
                }

                room.Name = model.Name;
                room.Location = model.Location;
                room.IsActive = model.IsActive;

                Context.SaveChanges();

                return Ok(model);
            }
        }

        //[HttpDelete("/room/delete/{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = roomRepository.GetRoomById(id);
            if (room == null)
            {
                return NotFound();
            }
            roomRepository.DeleteRoom(room);
            return Ok();
        }


    }
}
