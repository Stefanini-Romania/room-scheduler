using FluentValidation;
using RSService.BusinessLogic;
using RSService.Validation;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class EditAvailabilityValidator : AbstractValidator<EditAvailabilityViewModel>
    {
        private IRSManager _rsManager;

        public EditAvailabilityValidator(IRSManager rsManager)
        {
            _rsManager = rsManager;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(m => m.StartDate).Must(GoodTime).WithMessage(AvailabilityMessages.StartDateMinutesFormat);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(m => m.EndDate).Must(GoodTime).WithMessage(AvailabilityMessages.EndDateMinutesFormat);

            RuleFor(a => a.RoomId).NotEmpty().WithMessage(AvailabilityMessages.EmptyRoomId);
            RuleFor(a => a.RoomId).Must(ActiveRoom).WithMessage(AvailabilityMessages.InactiveRoom);

            RuleFor(a => a.Occurrence).Must(ValidOccurrence).WithMessage(AvailabilityMessages.IncorrectOccurrence);

            RuleFor(a => a.Status).Must(ValidStatus).WithMessage(AvailabilityMessages.IncorrectStatus);


        }

        private bool GoodTime(EditAvailabilityViewModel av, DateTime d)
        {

            return (d.Minute == 0 && d.Second == 0) ||
                   (d.Minute == 30 && d.Second == 0);
        }

        private bool ActiveRoom(EditAvailabilityViewModel av, int roomId)
        {
            return _rsManager.IsActiveRoom(roomId);
        }

        private bool ValidOccurrence(EditAvailabilityViewModel av, int occurrence)
        {
            if (occurrence !=0 && occurrence !=1 && occurrence !=2 && occurrence !=3 && occurrence !=4)
            {
                return false;
            }
            return true;
        }

        private bool ValidStatus(EditAvailabilityViewModel av, int status)
        {
            if (status != 0 && status != 1)
            {
                return false;
            }
            return true;
        }



    }
}
