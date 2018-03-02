using RSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class RoomService : IRoomService
    {
        private IRoomRepository roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public bool IsUniqueRoom(String name, String location, int roomid)
        {
            var rooms = roomRepository.GetRoomByNameAndLocation(name, location, roomid);

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
            var room = roomRepository.GetRoomByIdAndStatus(roomId, true);

            if (room == null)
            {
                return false;
            }
            return true;
        }
    }
}
