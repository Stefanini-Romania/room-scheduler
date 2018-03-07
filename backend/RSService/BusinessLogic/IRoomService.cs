using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IRoomService
    {

        bool IsActiveRoom(int roomId);

        bool IsUniqueRoom(RoomDto room);
    }
}
