using FluentValidation;
using RSService.BusinessLogic;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class CreateEventValidator : AbstractValidator<EventViewModel>
    {
        IRSManager rsManager;

        public CreateEventValidator(IRSManager _rsManager)
        {

            rsManager = _rsManager;

            RuleFor(m => m.StartDate)
                .NotEmpty().WithMessage("Start Date is required")    //.WithErrorCode("EmptyStartDate")
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .LessThan(m => m.EndDate.Value).WithMessage("Start Date must be less than End Date").When(m => m.EndDate.HasValue);
            //.Must(CanBook).WithMessage("You can't book more events for this day");

            RuleFor(m => m.EndDate)
                .NotEmpty().WithMessage("End Date is required")
                .GreaterThan(m => m.StartDate.Value).WithMessage("End Date must be greater than Start Date").When(m => m.StartDate.HasValue)
                .GreaterThanOrEqualTo(m => m.StartDate.Value.AddMinutes(30)).WithMessage("An event must be booked for at least 30 minutes").When(m => m.StartDate.HasValue)
                .LessThanOrEqualTo(m => m.StartDate.Value.AddHours(1)).WithMessage("An event can be booked for maximum 1 hour").When(m => m.StartDate.HasValue);
               
        }

        private bool CanBook(EventViewModel ev, DateTime? d)
        {
            return rsManager.GetTimeSpan((DateTime)ev.StartDate, (DateTime)ev.EndDate) <= rsManager.GetAvailableTime(ev.AttendeeId, (DateTime)ev.StartDate);
        }
    }
}
