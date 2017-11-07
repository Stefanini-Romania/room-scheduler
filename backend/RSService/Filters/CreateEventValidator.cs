using FluentValidation;
using RSData.Models;
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
                .NotEmpty().WithMessage(x => Validation.EventMessages.EmptyStartDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage(x => Validation.EventMessages.StartDatePast).When(m => m.EndDate.HasValue)
                .LessThan(DateTime.UtcNow.AddMonths(1)).WithMessage(x => Validation.EventMessages.StartDateFuture).When(m => m.EndDate.HasValue)
                .LessThan(m => m.EndDate.Value).WithMessage(x => Validation.EventMessages.GreaterThan).When(m => m.EndDate.HasValue)
                .LessThan(m => DateTime.UtcNow.AddMinutes(15)).WithMessage(x => Validation.EventMessages.CancellationTimeSpanLess).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);
            //.Must(CanBook).WithMessage("You can't book more events for this day");

            RuleFor(m => m.EndDate)
                .NotEmpty().WithMessage(x => Validation.EventMessages.EmptyEndDate)
                .GreaterThan(m => m.StartDate.Value).WithMessage(x => Validation.EventMessages.LessThan).When(m => m.StartDate.HasValue)
                .GreaterThanOrEqualTo(m => m.StartDate.Value.AddMinutes(30)).WithMessage(x => Validation.EventMessages.TimeSpanLess).When(m => m.StartDate.HasValue)
                .LessThanOrEqualTo(m => m.StartDate.Value.AddHours(1)).WithMessage(x => Validation.EventMessages.TimeSpanGreater).When(m => m.StartDate.HasValue);

            RuleFor(m => m.AttendeeId)
                .NotEmpty().WithMessage(x => Validation.EventMessages.EmptyAttendee)
                .Must(CanCancel).WithMessage(x => Validation.EventMessages.CancellationOwnBooking).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);
        }

        private bool CanBook(EventViewModel ev, DateTime? d)
        {
            return rsManager.GetTimeSpan((DateTime)ev.StartDate, (DateTime)ev.EndDate) <= rsManager.GetAvailableTime(ev.AttendeeId, (DateTime)ev.StartDate);
        }

        private bool CanCancel (EventViewModel ev, int attendee)
        {
            return rsManager.CanCancel((DateTime)ev.StartDate, (DateTime)ev.EndDate, ev.RoomId, attendee);
        }

     

    }
}
