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
    public class AddAvailabilityValidator : AbstractValidator<AvailabilityViewModel>
    {
        private IRSManager _rsManager;

        public AddAvailabilityValidator(IRSManager rsManager)
        {
            _rsManager = rsManager;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(m => m.StartDate).Must(GoodTime).WithMessage(AvailabilityMessages.StartDateMinutesFormat);



            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(m => m.EndDate).Must(GoodTime).WithMessage(AvailabilityMessages.EndDateMinutesFormat);

            RuleFor(a => a.RoomId).NotEmpty().WithMessage(AvailabilityMessages.EmptyRoomId);
            RuleFor(a => a.RoomId).Must(ActiveRoom).WithMessage(AvailabilityMessages.InactiveRoom);

            RuleFor(a => a.DaysOfWeek).NotEmpty().WithMessage(AvailabilityMessages.EmptyDayOfWeek);
            RuleFor(a => a.DaysOfWeek).Must(ValidDays).WithMessage(AvailabilityMessages.IncorrectDayOfWeek);

            RuleFor(a => a.Occurrence).Must(ValidOccurrence).WithMessage(AvailabilityMessages.IncorrectOccurrence);


        }

        private bool GoodTime(AvailabilityViewModel av, DateTime d)
        {
            
            return (d.Minute == 0 && d.Second == 0) ||
                   (d.Minute == 30 && d.Second == 0);
        }

        private bool ActiveRoom(AvailabilityViewModel av, int roomId)
        {
            return _rsManager.IsActiveRoom(roomId);
        }

        private bool ValidDays(AvailabilityViewModel av, int[] daysOfWeek)
        {
            foreach(var day in daysOfWeek)
            {
                if(day < 1 || day > 5)
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidOccurrence(AvailabilityViewModel av, int occurrence)
        {
            if (occurrence < 1 || occurrence > 4)
            {
                return false;
            }
            return true;
        }

    }
}
