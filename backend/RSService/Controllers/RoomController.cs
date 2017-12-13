﻿using AutoMapper;
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

        public RoomController()
        {
            this.roomRepository = new RoomRepository(Context);
        }

        [HttpGet("/room/list")]
        public IActionResult GetRooms(bool isActive)
        {
            if (!isActive)
            {
                var rooms = roomRepository.GetRooms();
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
            else
            {
                var rooms = roomRepository.GetRoomsByStatus(isActive);
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

            
        }

        [HttpPost("/room/add")]
        public IActionResult AddRoom([FromBody]RoomDto model)
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
        public IActionResult UpdateRoom(int id, [FromBody] RoomDto roomDto)
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

                room.Name = roomDto.Name;
                room.Location = roomDto.Location;
                room.IsActive = roomDto.IsActive;

                Context.SaveChanges();

                return Ok(roomDto);
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
