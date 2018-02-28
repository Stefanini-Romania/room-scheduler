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
    public class EditAvailabilityValidator : AbstractValidator<EditAvailabilityDto>
    {
        private IRoomService roomService;
        private IAvailabilityService availabilityService;

        public EditAvailabilityValidator(IRoomService roomService, IAvailabilityService availabilityService)
        {
            this.roomService = roomService;
            this.availabilityService = availabilityService;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(m => m.StartDate).Must(GoodStartTime).WithMessage(AvailabilityMessages.IncorrectStartTime);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(m => m.EndDate).Must(GoodEndTime).WithMessage(AvailabilityMessages.IncorrectEndTime);
            RuleFor(a => a.EndDate).GreaterThan(a => a.StartDate).WithMessage(AvailabilityMessages.LessThanStartDate);

            RuleFor(a => a.RoomId).NotEmpty().WithMessage(AvailabilityMessages.EmptyRoomId);
            When(a => a.RoomId != 0, () =>
            {
                RuleFor(a => a.RoomId).Must(ActiveRoom).WithMessage(AvailabilityMessages.InactiveRoom);
            });

            RuleFor(a => a.Occurrence).Must(ValidOccurrence).WithMessage(AvailabilityMessages.IncorrectOccurrence);

            RuleFor(a => a.Status).Must(ValidStatus).WithMessage(AvailabilityMessages.IncorrectStatus);


        }

        private bool GoodStartTime(EditAvailabilityDto av, DateTime d)
        {
            return availabilityService.IsGoodStartTime(av);
        }

        private bool GoodEndTime(EditAvailabilityDto av, DateTime d)
        {
            return availabilityService.IsGoodEndTime(av);
        }

        private bool ActiveRoom(EditAvailabilityDto av, int roomId)
        {
            return roomService.IsActiveRoom(roomId);
        }

        private bool ValidOccurrence(EditAvailabilityDto av, int occurrence)
        {
            return availabilityService.ValidOccurrence(av);
        }

        private bool ValidStatus(EditAvailabilityDto av, int status)
        {
            return availabilityService.ValidStatus(av);
        }



    }
}
