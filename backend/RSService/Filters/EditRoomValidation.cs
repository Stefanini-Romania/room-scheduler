using FluentValidation;
using RSService.BusinessLogic;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class EditRoomValidation: AbstractValidator<EditRoomViewModel>
    {
        private IRSManager rsManager;
        public EditRoomValidation(IRSManager rSManager)
        {
            rsManager = rSManager;

            RuleFor(m => m.Name).NotEmpty().WithMessage(x => Validation.RoomMessages.EmptyRoomName);
            RuleFor(m => m.Location).NotEmpty().WithMessage(x => Validation.RoomMessages.EmptyRoomLocation);
            RuleFor(m => m.Name).Must(IsUniqueRoom).WithMessage(x => Validation.RoomMessages.UniqueRoom);


        }

        private bool IsUniqueRoom(EditRoomViewModel m, String roomName)
        {
            return rsManager.IsUniqueRoom(roomName, m.Location);
        }
    }
}
