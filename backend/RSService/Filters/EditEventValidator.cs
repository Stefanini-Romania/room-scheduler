using FluentValidation;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class EditEventValidator : AbstractValidator<EditEventViewModel>
    {
        public EditEventValidator()
        {

            // ---------------------------AttendeeId---------------------------
            //RuleFor(m => m.AttendeeId)
            //    .Must(CanCancel).WithMessage(x => Validation.EventMessages.CancellationOwnBooking).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);

            //// ---------------------------RoomId---------------------------
            //RuleFor(m => m.RoomId)
            //    .Must(IsNotPenalized).WithMessage(x => Validation.EventMessages.Penalized);

        }



    }
}
