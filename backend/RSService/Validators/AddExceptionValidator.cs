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
    public class AddExceptionValidator : AbstractValidator<AvailabilityExceptionDto>
    {
        private IAvailabilityService availabilityService;

        public AddExceptionValidator(IAvailabilityService availabilityService)
        {
            this.availabilityService = availabilityService;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(a => a.StartDate).Must(GoodStartTime).WithMessage(AvailabilityMessages.IncorrectStartTime);
            RuleFor(a => a.StartDate).Must(GoodStartDate).WithMessage(AvailabilityMessages.InvalidTimeSpan);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(a => a.EndDate).Must(GoodEndTime).WithMessage(AvailabilityMessages.IncorrectEndTime);
            RuleFor(a => a.EndDate).Must(GoodEndDate).WithMessage(AvailabilityMessages.InvalidTimeSpan);
            RuleFor(a => a.EndDate).GreaterThan(a => a.StartDate).WithMessage(AvailabilityMessages.LessThanStartDate);

        }

        private bool GoodStartTime(AvailabilityExceptionDto ex, DateTime d)
        {
            return availabilityService.IsGoodStartTime(ex);
        }

        private bool GoodEndTime(AvailabilityExceptionDto ex, DateTime d)
        {
            return availabilityService.IsGoodEndTime(ex);
        }

        private bool GoodStartDate(AvailabilityExceptionDto ex, DateTime d)
        {
            return (d.Date == ex.EndDate.Date);
        }

        private bool GoodEndDate(AvailabilityExceptionDto ex, DateTime d)
        {
            return (d.Date == ex.StartDate.Date);
        }

    }
}
