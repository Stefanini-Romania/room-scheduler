using FluentValidation;
using RSData.Models;
using RSService.BusinessLogic;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class CreateEventValidator : AbstractValidator<AddEventDto>
    {

        IEventService eventService;
        IPenaltyService penaltyService;

        public CreateEventValidator(IEventService eventService, IPenaltyService penaltyService)
        {
            this.eventService = eventService;
            this.penaltyService = penaltyService;

            // ---------------------------StartDate---------------------------

            RuleFor(m => m.StartDate).NotEmpty().WithMessage(x => Validation.EventMessages.EmptyStartDate);

            When(m => m.StartDate.HasValue, () => {
                RuleFor(m => m.StartDate).Must(CanBook).WithMessage(x => Validation.EventMessages.Limit).When(m => m.EndDate.HasValue);

                RuleFor(m => m.StartDate).GreaterThanOrEqualTo(DateTime.UtcNow.ToLocalTime()).WithMessage(x => Validation.EventMessages.StartDatePast).When(m => m.EndDate.HasValue);

                // RuleFor(m => m.StartDate).LessThan(DateTime.Now.AddMonths(2)).WithMessage(x => Validation.EventMessages.StartDateFuture).When(m => m.EndDate.HasValue);


                RuleFor(m => m.StartDate).Must(GoodStartTime).WithMessage(x => Validation.EventMessages.StartDateSpecific);

                RuleFor(x => x.StartDate).Must(IsAvailable).WithMessage(x => Validation.EventMessages.NotAvailable).When(x => x.EndDate.HasValue);

                RuleFor(x => x.StartDate).Must(DayOfWeek).WithMessage(x => Validation.EventMessages.DayOfWeekWeekend);

                RuleFor(x => x.StartDate).Must(TwoMonths).WithMessage(x => Validation.EventMessages.StartDateFuture);

                RuleFor(x => x.StartDate).Must(HourAvailable).WithMessage(x => Validation.EventMessages.NotAvailable).When(x => x.EndDate.HasValue);

                // ---------------------------RoomId---------------------------
                RuleFor(m => m.RoomId).Must(IsNotPenalized).WithMessage(x => Validation.EventMessages.Penalized).When(x => x.EndDate.HasValue);
            });

            // ---------------------------EndDate---------------------------

            RuleFor(m => m.EndDate).NotEmpty().WithMessage(x => Validation.EventMessages.EmptyEndDate);

            When(m => m.EndDate.HasValue, () => {

                RuleFor(m => m.EndDate).Must(TimeSpanOfEvent).WithMessage(x => Validation.EventMessages.InvalidTimeSpan).When(m => m.StartDate.HasValue);

                RuleFor(m => m.EndDate).Must(GoodEndTime).WithMessage(x => Validation.EventMessages.EndDateSpecific);
            });

            // ---------------------------EventStatus---------------------------
            RuleFor(m => m.EventStatus).Equal((int)EventStatusEnum.waiting).WithMessage(x => Validation.EventMessages.InvalidEventStatus);

        }


        private bool CanBook(AddEventDto ev, DateTime? d)
        {
            double eventTimeSpan = eventService.GetTimeSpan((DateTime)ev.StartDate, (DateTime)ev.EndDate);

            int availableTimeSpan = eventService.GetAvailableTime(ev.AttendeeId, (DateTime)ev.StartDate);

            return eventTimeSpan <= availableTimeSpan;
        }

        private bool TimeSpanOfEvent(AddEventDto ev, DateTime? d)
        {
            double eventTimeSpan = eventService.GetTimeSpan((DateTime)ev.StartDate, (DateTime)ev.EndDate);

            if (eventTimeSpan != 30 && eventTimeSpan != 60)
            {
                return false;
            }
            return true;
        }
        
        private bool GoodStartTime(AddEventDto ev, DateTime? d)
        {
            if (d.HasValue) {
                return d.Value.Hour >= 9 && d.Value.Hour <= 17 && d.Value.Second == 0 && (d.Value.Minute == 0 || d.Value.Minute == 30);
            }
            return false;
        }

        private bool GoodEndTime(AddEventDto ev, DateTime? d)
        {
            if (d.HasValue)
            {
                return d.Value.Hour >= 9 && d.Value.Hour <= 17 && d.Value.Second == 0 && (d.Value.Minute == 0 || d.Value.Minute == 30) ||
                       d.Value.Hour == 18 && d.Value.Second == 0 && d.Value.Minute == 0;
            }
            return false;
        }

        private bool IsAvailable(AddEventDto ev, DateTime? d)
        {
            return eventService.CheckAvailability((DateTime)ev.StartDate, (DateTime)ev.EndDate, ev.RoomId);
        }

        private bool HourAvailable(AddEventDto ev, DateTime? d)
        {
            return eventService.HourCheck((DateTime)ev.StartDate, (DateTime)ev.EndDate, ev.RoomId);
        }

        private bool IsNotPenalized(AddEventDto ev, int roomId)
        {
            if (penaltyService.HasPenalty(ev.AttendeeId, (DateTime)ev.StartDate, roomId))
            {
                return false;
            }
            return true;
        }
        private bool DayOfWeek(AddEventDto ev, DateTime? date)
        {
            if ((int)ev.StartDate.Value.DayOfWeek >= 1 && (int)ev.StartDate.Value.DayOfWeek <= 5)
                return true;

            return false;
        }

        private bool TwoMonths(AddEventDto ev, DateTime? date)
        {
            if((ev.StartDate.Value.Month <= DateTime.Now.Month+1)&&(ev.StartDate.Value.Day >=1 && ev.StartDate.Value.Day <=31)&&(ev.StartDate.Value.Year==DateTime.Now.Year))
                    return true;

            if ((ev.StartDate.Value.Year == DateTime.Now.Year + 1) && (ev.StartDate.Value.Month == 01 && DateTime.Now.Month == 12))
                return true;
            return false;
        }


    }
}
