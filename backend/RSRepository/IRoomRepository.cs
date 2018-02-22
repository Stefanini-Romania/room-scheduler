using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IRoomRepository
    {
        List<Room> GetRooms();
        List<Room> GetRoomsByStatus(Nullable<bool> isActive = null);
        Room GetRoomById(int id);
        Room GetRoomByNameAndLocation(String name, String location,int roomId);
        Room GetRoomByIdAndStatus(int roomId, bool status);
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(Room room);
    }
}
