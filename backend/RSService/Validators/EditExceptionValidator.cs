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
    public class EditExceptionValidator : AbstractValidator<EditExceptionDto>
    {
        IAvailabilityService availabilityService;

        public EditExceptionValidator(IAvailabilityService availabilityService)
        {
            this.availabilityService = availabilityService;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(m => m.StartDate).Must(GoodStartTime).WithMessage(AvailabilityMessages.IncorrectStartTime);
            RuleFor(a => a.StartDate).Must(GoodStartDate).WithMessage(AvailabilityMessages.InvalidTimeSpan);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(m => m.EndDate).Must(GoodEndTime).WithMessage(AvailabilityMessages.IncorrectEndTime);
            RuleFor(a => a.EndDate).Must(GoodEndDate).WithMessage(AvailabilityMessages.InvalidTimeSpan);
            RuleFor(a => a.EndDate).GreaterThan(a => a.StartDate).WithMessage(AvailabilityMessages.LessThanStartDate);

            RuleFor(a => a.Status).Must(ValidStatus).WithMessage(AvailabilityMessages.IncorrectStatus);
        }

        private bool GoodStartTime(EditExceptionDto ex, DateTime d)
        {
            return availabilityService.IsGoodStartTime(ex);
        }

        private bool GoodEndTime(EditExceptionDto ex, DateTime d)
        {
            return availabilityService.IsGoodStartTime(ex);
        }

        private bool GoodStartDate(EditExceptionDto ex, DateTime d)
        {
            return availabilityService.IsGoodStartDate(ex);
        }

        private bool GoodEndDate(EditExceptionDto ex, DateTime d)
        {
            return availabilityService.IsGoodEndDate(ex);
        }

        private bool ValidStatus(EditExceptionDto ex, int status)
        {
            return availabilityService.IsValidStatus(ex);
        }
    }
}
