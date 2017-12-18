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

            When(m => m.StartDate.HasValue, () => {
                RuleFor(m => m.StartDate).Must(CanBook).WithMessage(x => Validation.EventMessages.Limit).When(m => m.EndDate.HasValue);

                RuleFor(m => m.StartDate).GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(2)).WithMessage(x => Validation.EventMessages.StartDatePast).When(m => m.EndDate.HasValue);

               // RuleFor(m => m.StartDate).LessThan(DateTime.Now.AddMonths(2)).WithMessage(x => Validation.EventMessages.StartDateFuture).When(m => m.EndDate.HasValue);

                RuleFor(m => m.StartDate).GreaterThanOrEqualTo(m => DateTime.UtcNow.AddMinutes(15)).WithMessage(x => Validation.EventMessages.CancellationTimeSpanLess).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);

                RuleFor(m => m.StartDate).Must(GoodTime).WithMessage(x => Validation.EventMessages.StartDateSpecific);

                RuleFor(x => x.StartDate).Must(AvailabilityTimeS).WithMessage(x => Validation.EventMessages.StartDateAvailabilityRoom);

                RuleFor(x => x.StartDate).Must(IsAvailable).WithMessage(x => Validation.EventMessages.NotAvailable);

                RuleFor(x => x.StartDate).Must(DayOfWeek).WithMessage(x => Validation.EventMessages.DayOfWeekWeekend);

                RuleFor(x => x.StartDate).Must(TwoMonths).WithMessage(x => Validation.EventMessages.StartDateFuture);

                RuleFor(x => x.StartDate).Must(HourAvailable).WithMessage(x => Validation.EventMessages.NotAvailable);    





            });

            // ---------------------------EndDate---------------------------

            RuleFor(m => m.EndDate).NotNull().WithMessage(x => Validation.EventMessages.EmptyEndDate);

            When(m => m.EndDate.HasValue, () => {

                RuleFor(m => m.EndDate).Must(TimeSpanOfEvent).WithMessage(x => Validation.EventMessages.InvalidTimeSpan).When(m => m.StartDate.HasValue);

                RuleFor(m => m.EndDate).Must(AvailabilityTimeE).WithMessage(x => Validation.EventMessages.EndDateAvailabilityRoom);

                RuleFor(m => m.EndDate).Must(GoodTime).WithMessage(x => Validation.EventMessages.EndDateSpecific);
            });

            // ---------------------------AttendeeId---------------------------
            RuleFor(m => m.AttendeeId)
                .Must(CanCancel).WithMessage(x => Validation.EventMessages.CancellationOwnBooking).When(m => m.EventStatus == (int)EventStatusEnum.cancelled);

            // ---------------------------RoomId---------------------------
            RuleFor(m => m.RoomId)
                .Must(IsNotPenalized).WithMessage(x => Validation.EventMessages.Penalized);
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

        private bool AvailabilityTimeS(EventViewModel ev, DateTime?d)
        {
            if(d.HasValue)
            {
                return ((d.GetValueOrDefault().Hour >= 09 && d.GetValueOrDefault().Hour <= 17));
                    //&& d.GetValueOrDefault().Hour < 13) ||( d.GetValueOrDefault().Hour >= 14 
            }
            return false;
        }

        private bool AvailabilityTimeE(EventViewModel ev, DateTime? d)
        {
            if (d.HasValue)
            {
               // if (d.GetValueOrDefault().Hour == 13 && d.GetValueOrDefault().Minute == 30)
              //      return false;

                return (d.GetValueOrDefault().Hour <= 18 && d.GetValueOrDefault().Minute < 30)
                     || (d.GetValueOrDefault().Hour <= 17 && d.GetValueOrDefault().Minute <= 30);
            }
            return false;
        }

        private bool IsAvailable(EventViewModel ev, DateTime? d)
        {
            return rsManager.CheckAvailability((DateTime)ev.StartDate, (DateTime)ev.EndDate, ev.RoomId);
        }

        private bool HourAvailable(EventViewModel ev, DateTime? d)
        {
            return rsManager.HourCheck((DateTime)ev.StartDate, (DateTime)ev.EndDate, ev.RoomId);
        }

        private bool IsNotPenalized(EventViewModel ev, int roomId)
        {
            if (rsManager.HasPenalty(ev.AttendeeId, (DateTime)ev.StartDate, roomId))
            {
                return false;
            }
            return true;
        }
        private bool DayOfWeek(EventViewModel ev, DateTime? date)
        {
            if ((int)ev.StartDate.Value.DayOfWeek >= 1 && (int)ev.StartDate.Value.DayOfWeek <= 5)
                return true;

            return false;
        }

        private bool TwoMonths(EventViewModel ev, DateTime? date)
        {
            if((ev.StartDate.Value.Month <= DateTime.Now.Month+1)&&(ev.StartDate.Value.Day >=1 && ev.StartDate.Value.Day <=31)&&(ev.StartDate.Value.Year==DateTime.Now.Year))
                    return true;

            if ((ev.StartDate.Value.Year == DateTime.Now.Year + 1) && (ev.StartDate.Value.Month == 01 && DateTime.Now.Month == 12))
                return true;
            return false;
        }

        

    }
}
