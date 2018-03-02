using FluentValidation;
using RSService.BusinessLogic;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class EditRoomValidator: AbstractValidator<RoomDto>
    {
        private IRoomService _roomService;
        public EditRoomValidator(IRoomService roomService)
        {
            _roomService = roomService;

            RuleFor(m => m.Name).NotEmpty().WithMessage(x => Validation.RoomMessages.EmptyRoomName);
            RuleFor(m => m.Name).MaximumLength(30).WithMessage(x => Validation.RoomMessages.RoomNameLong);
            RuleFor(m => m.Location).MaximumLength(30).WithMessage(x => Validation.RoomMessages.LocationNameLong);
            RuleFor(m => m.Location).NotEmpty().WithMessage(x => Validation.RoomMessages.EmptyRoomLocation);
            RuleFor(m => m.Name).Must(IsUniqueRoom).WithMessage(x => Validation.RoomMessages.UniqueRoom);


        }

        private bool IsUniqueRoom(RoomDto m, String roomName)
        {
            return _roomService.IsUniqueRoom(roomName, m.Location,m.Id);
        }
    }
}
