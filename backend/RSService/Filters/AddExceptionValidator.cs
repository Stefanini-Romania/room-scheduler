using FluentValidation;
using RSService.DTO;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class AddExceptionValidator : AbstractValidator<AvailabilityExceptionDto>
    {

        public AddExceptionValidator()
        {
            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(a => a.StartDate).Must(GoodStartTime).WithMessage(AvailabilityMessages.IncorrectStartTime);
            RuleFor(a => a.StartDate).Must(GoodStartDate).WithMessage(AvailabilityMessages.InvalidTimeSpan);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(a => a.EndDate).Must(GoodEndTime).WithMessage(AvailabilityMessages.IncorrectEndTime);
            RuleFor(a => a.EndDate).Must(GoodEndDate).WithMessage(AvailabilityMessages.InvalidTimeSpan);

        }

        private bool GoodStartTime(AvailabilityExceptionDto ex, DateTime d)
        {

            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30);
        }

        private bool GoodEndTime(AvailabilityExceptionDto ex, DateTime d)
        {
            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30) ||
                   d.Hour >= 9 && d.Hour == 18 && d.Second == 0 && d.Minute == 0;
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
