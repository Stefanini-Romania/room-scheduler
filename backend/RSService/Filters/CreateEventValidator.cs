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

            // ---------------------------StartDate---------------------------


            RuleFor(m => m.StartDate).NotNull().WithMessage(x => Validation.EventMessages.EmptyStartDate);
            //.Must(GoodTime).WithMessage(x => Validation.EventMessages.StartDateSpecific);

            When(m => m.StartDate.HasValue, () => {
                RuleFor(m => m.StartDate).Must(CanBook).WithMessage(x => Validation.EventMessages.Limit).When(m => m.EndDate.HasValue);

                RuleFor(m => m.StartDate).GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage(x => Validation.EventMessages.StartDatePast).When(m => m.EndDate.HasValue);

                RuleFor(m => m.StartDate).LessThan(DateTime.UtcNow.AddMonths(1)).WithMessage(x => Validation.EventMessages.StartDateFuture).When(m => m.EndDate.HasValue);

                RuleFor(m => m.StartDate).LessThan(m => DateTime.UtcNow.AddMinutes(15)).WithMessage(x => Validation.EventMessages.CancellationTimeSpanLess).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);
            });

            // ---------------------------EndDate---------------------------

            RuleFor(m => m.EndDate).NotNull().WithMessage(x => Validation.EventMessages.EmptyEndDate);

            When(m => m.EndDate.HasValue, () => {
                RuleFor(m => m.EndDate).Must(TimeSpanOfEvent).WithMessage(x => Validation.EventMessages.InvalidTimeSpan).When(m => m.StartDate.HasValue);
            });

            // ---------------------------AttendeeId---------------------------

            RuleFor(m => m.AttendeeId)
                .NotNull().WithMessage(x => Validation.EventMessages.EmptyAttendee);
            RuleFor(m => m.AttendeeId)
                .Must(CanCancel).WithMessage(x => Validation.EventMessages.CancellationOwnBooking).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);
        }


        private bool CanBook(EventViewModel ev, DateTime? d)
        {
            double eventTimeSpan = rsManager.GetTimeSpan((DateTime)ev.StartDate, (DateTime)ev.EndDate);

            int availableTimeSpan = rsManager.GetAvailableTime(ev.AttendeeId, (DateTime)ev.StartDate);

            return eventTimeSpan <= availableTimeSpan;
        }

        private bool CanCancel (EventViewModel ev, int attendee)
        {
            return rsManager.CanCancel((DateTime)ev.StartDate, (DateTime)ev.EndDate, ev.RoomId, attendee);
        }

        private bool TimeSpanOfEvent(EventViewModel ev, DateTime? d)
        {
            double eventTimeSpan = rsManager.GetTimeSpan((DateTime)ev.StartDate, (DateTime)ev.EndDate);

            if (eventTimeSpan != 30 && eventTimeSpan != 60)
            {
                return false;
            }
            return true;
        }
        
        private bool GoodTime(EventViewModel ev, DateTime? d)
        {
            if (d.HasValue) {
                return (d.GetValueOrDefault().Minute == 0 && d.GetValueOrDefault().Second == 0) 
                    || (d.GetValueOrDefault().Minute == 30 && d.GetValueOrDefault().Second == 0);
            }
            return false;
        }
     

    }
}
