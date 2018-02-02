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

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);

            //RuleFor(a => a.RoomId).       // diferit de zero




        }


    }
}
