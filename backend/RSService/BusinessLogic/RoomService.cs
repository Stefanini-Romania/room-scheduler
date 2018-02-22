using RSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class RoomService : IRoomService
    {
        private IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public bool IsUniqueRoom(String name, String location, int roomid)
        {
            var rooms = _roomRepository.GetRoomByNameAndLocation(name, location, roomid);

            if (rooms == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsActiveRoom(int roomId)
        {
            var room = _roomRepository.GetRoomByIdAndStatus(roomId, true);

            if (room == null)
            {
                return false;
            }
            return true;
        }
    }
}
