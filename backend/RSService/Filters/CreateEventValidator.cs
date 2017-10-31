using FluentValidation;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class CreateEventValidator : AbstractValidator<EventViewModel>
    {
        public CreateEventValidator()
        {
            RuleFor(m => m.StartDate)
                .NotEmpty().WithMessage("Start Date is required")
                .GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(m => m.EndDate)
                .NotEmpty().WithMessage("End Date is required")
                .GreaterThanOrEqualTo(m => m.StartDate.Value).WithMessage("End Date must be greater than Start Date").When(m => m.StartDate.HasValue);
        }

    }
}
