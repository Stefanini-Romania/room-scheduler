using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System.Linq;

namespace RSRepository
{
    public class RoomRepository : IRoomRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<Room> rooms;

        public RoomRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            rooms = context.Set<Room>();
        }


        public IEnumerable<Room> GetRooms()
        {
            return rooms.AsEnumerable();
        }

        public Room GetRoomById(int id)
        {
            return rooms.SingleOrDefault(s => s.Id == id);
        }

        public void AddRoom(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException("Add a null room");
            }
            rooms.Add(room);
            context.SaveChanges();
        }

        public void UpdateRoom(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException("Update a null room");
            }
            context.SaveChanges();
        }

        public void DeleteRoom(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException("Add a null room");
            }
            rooms.Remove(room);
            context.SaveChanges();
        }

       

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        
    }
}
