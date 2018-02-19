using FluentValidation;
using RSService.BusinessLogic;
using RSService.DTO;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class AddAvailabilityValidator : AbstractValidator<AddAvailabilityDto>
    {
        private IRSBusiness _rsBusiness;

        public AddAvailabilityValidator(IRSBusiness rsBusiness)
        {
            _rsBusiness = rsBusiness;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(a => a.StartDate).Must(GoodStartTime).WithMessage(AvailabilityMessages.IncorrectStartTime);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(a => a.EndDate).Must(GoodEndTime).WithMessage(AvailabilityMessages.IncorrectEndTime);
            RuleFor(a => a.EndDate).GreaterThan(a => a.StartDate).WithMessage(AvailabilityMessages.LessThanStartDate);

            RuleFor(a => a.RoomId).NotEmpty().WithMessage(AvailabilityMessages.EmptyRoomId);
            When(a => a.RoomId != 0, () =>
            {
               RuleFor(a => a.RoomId).Must(ActiveRoom).WithMessage(AvailabilityMessages.InactiveRoom);
            });

            RuleFor(a => a.DaysOfWeek).NotEmpty().WithMessage(AvailabilityMessages.EmptyDayOfWeek);
            RuleFor(a => a.DaysOfWeek).Must(ValidDays).WithMessage(AvailabilityMessages.IncorrectDayOfWeek);

            RuleFor(a => a.Occurrence).Must(ValidOccurrence).WithMessage(AvailabilityMessages.IncorrectOccurrence);
        }

        private bool GoodStartTime(AddAvailabilityDto av, DateTime d)
        {

            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30);
        }

        private bool GoodEndTime(AddAvailabilityDto av, DateTime d)
        {
            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30) ||
                   d.Hour >=9 && d.Hour == 18 && d.Second == 0 && d.Minute == 0;
        }

        private bool ActiveRoom(AddAvailabilityDto av, int roomId)
        {
            return _rsBusiness.IsActiveRoom(roomId);
        }

        private bool ValidDays(AddAvailabilityDto av, int[] daysOfWeek)
        {
            if (daysOfWeek == null)
            {
                return false;
            }
            if (daysOfWeek.Length > 5)
            {
                return false;
            }

            foreach (var day in daysOfWeek)
            {
                if (day != 1 && day != 2 && day != 3 && day != 4 && day != 5)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidOccurrence(AddAvailabilityDto av, int occurrence)
        {
            if (occurrence != 1 && occurrence != 2 && occurrence != 3 && occurrence != 4)
            {
                return false;
            }

            return true;
        }



    }
}
