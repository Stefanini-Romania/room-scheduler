﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.DTO;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RSService.Controllers
{
    public class RoomController : ValidationController
    {
        private readonly IRoomRepository roomRepository;
        private readonly IUserRepository userRepository;
        private readonly RoomPlannerDevContext context;

        public RoomController(RoomPlannerDevContext context)
        {
            this.context = context;
            roomRepository = new RoomRepository(context);
            userRepository = new UserRepository(context);
        }

        [HttpGet("/room/list")]
        public IActionResult GetRooms()
        {

            User currentUser = null;

            var schedulerIdentity = SchedulerIdentity.Current(HttpContext);

            if (schedulerIdentity != null)
            {
                var currentUserId = schedulerIdentity.UserId;
                currentUser = userRepository.GetUserById(currentUserId);
            }

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
        public IActionResult AddRoom([FromBody] RoomDto model) 
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Room);
            }

            if (model.Name == null || model.Location == null)
            {
                return BadRequest();
            }

            Room newRoom = new Room(model.Name, model.Location, true);  
            roomRepository.AddRoom(newRoom);

            context.SaveChanges();
            return Ok(model);
        }

        [HttpPut("/room/edit/{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult UpdateRoom(int id, [FromBody] RoomDto model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.RoomEdit);
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

                context.SaveChanges();

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
