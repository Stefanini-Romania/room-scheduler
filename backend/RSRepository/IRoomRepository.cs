using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IRoomRepository
    {
        List<Room> GetRooms();
        List<Room> GetRooms(int limit, int page);
        Room GetRoomById(int id);
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(Room room);
        void SaveChanges();
    }
}
