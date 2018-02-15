using FluentValidation;
using RSService.BusinessLogic;
using RSService.DTO;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class EditAvailabilityValidator : AbstractValidator<EditAvailabilityDto>
    {
        private IRSManager _rsManager;

        public EditAvailabilityValidator(IRSManager rsManager)
        {
            _rsManager = rsManager;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(m => m.StartDate).Must(GoodStartTime).WithMessage(AvailabilityMessages.IncorrectStartTime);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(m => m.EndDate).Must(GoodEndTime).WithMessage(AvailabilityMessages.IncorrectEndTime);
            RuleFor(a => a.EndDate).GreaterThan(a => a.StartDate).WithMessage(AvailabilityMessages.LessThanStartDate);

            RuleFor(a => a.RoomId).NotEmpty().WithMessage(AvailabilityMessages.EmptyRoomId);
            RuleFor(a => a.RoomId).Must(ActiveRoom).WithMessage(AvailabilityMessages.InactiveRoom);

            RuleFor(a => a.Occurrence).Must(ValidOccurrence).WithMessage(AvailabilityMessages.IncorrectOccurrence);

            RuleFor(a => a.Status).Must(ValidStatus).WithMessage(AvailabilityMessages.IncorrectStatus);


        }

        private bool GoodStartTime(EditAvailabilityDto av, DateTime d)
        {

            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30);
        }

        private bool GoodEndTime(EditAvailabilityDto av, DateTime d)
        {
            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30) ||
                   d.Hour >= 9 && d.Hour == 18 && d.Second == 0 && d.Minute == 0;
        }

        private bool ActiveRoom(EditAvailabilityDto av, int roomId)
        {
            return _rsManager.IsActiveRoom(roomId);
        }

        private bool ValidOccurrence(EditAvailabilityDto av, int occurrence)
        {
            if (occurrence !=0 && occurrence !=1 && occurrence !=2 && occurrence !=3 && occurrence !=4)
            {
                return false;
            }
            return true;
        }

        private bool ValidStatus(EditAvailabilityDto av, int status)
        {
            if (status != 0 && status != 1)
            {
                return false;
            }
            return true;
        }



    }
}
