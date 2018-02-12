using FluentValidation;
using RSData.Models;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class EditEventValidator : AbstractValidator<EditEventDto>
    {
        public EditEventValidator()
        {
            RuleFor(m => m.StartDate).GreaterThanOrEqualTo(m => DateTime.UtcNow.ToLocalTime().AddMinutes(15)).WithMessage(x => Validation.EventMessages.CancellationTimeSpanLess).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);
            
            
            // //---------------------------AttendeeId-------------------------- -
            //RuleFor(m => m.AttendeeId)
            //    .Must(CanCancel).WithMessage(x => Validation.EventMessages.CancellationRight).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);

        }


    }
}
