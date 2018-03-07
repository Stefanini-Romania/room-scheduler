using FluentValidation;
using RSService.BusinessLogic;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class RoomValidator: AbstractValidator<RoomDto>
    {
        private IRoomService _roomService;
        public RoomValidator(IRoomService roomService)
        {
            _roomService = roomService;

            RuleFor(m => m.Name).NotEmpty().WithMessage(x => Validation.RoomMessages.EmptyRoomName);
            RuleFor(m => m.Name).Must(RoomNameMaxLength).WithMessage(x => Validation.RoomMessages.RoomNameLong);
            RuleFor(m => m.Location).Must(LocationNameMaxLength).WithMessage(x => Validation.RoomMessages.LocationNameLong);
            RuleFor(m => m.Location).NotEmpty().WithMessage(x => Validation.RoomMessages.EmptyRoomLocation);
            RuleFor(m => m.Name).Must(IsUniqueRoom).WithMessage(x => Validation.RoomMessages.UniqueRoom);


        }

        private bool IsUniqueRoom(RoomDto room, String roomName)
        {
            return _roomService.IsUniqueRoom(room);
        }

        private bool RoomNameMaxLength(String roomName)
        {
            return _roomService.RoomNameMaxLength(roomName);
        }

        private bool LocationNameMaxLength(String locName)
        {
            return _roomService.LocationNameMaxLength(locName);
        }
    }
}
