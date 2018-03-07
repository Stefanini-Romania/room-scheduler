using RSRepository;
using RSService.DTO;
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

        public bool IsUniqueRoom(RoomDto room)
        {
            var rooms = roomRepository.GetRoomByNameAndLocation(room.Name, room.Location, room.Id);

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


        public bool RoomNameMaxLength(String roomName)
        {
            if (roomName.Length > 30)
                return false;
            return true;
        }

        public bool LocationNameMaxLength(String locationName)
        {
            if (locationName.Length > 30)
                return false;
            return true;
        }
    }
}
