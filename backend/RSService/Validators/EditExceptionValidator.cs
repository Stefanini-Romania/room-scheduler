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
        private IRSBusiness _rsBusiness;

        public EditExceptionValidator(IRSBusiness rsBusiness)
        {
            _rsBusiness = rsBusiness;

            RuleFor(a => a.StartDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyStartDate);
            RuleFor(m => m.StartDate).Must(GoodStartTime).WithMessage(AvailabilityMessages.IncorrectStartTime);

            RuleFor(a => a.EndDate).NotEmpty().WithMessage(AvailabilityMessages.EmptyEndDate);
            RuleFor(m => m.EndDate).Must(GoodEndTime).WithMessage(AvailabilityMessages.IncorrectEndTime);
            RuleFor(a => a.EndDate).GreaterThan(a => a.StartDate).WithMessage(AvailabilityMessages.LessThanStartDate);

            RuleFor(a => a.Status).Must(ValidStatus).WithMessage(AvailabilityMessages.IncorrectStatus);
        }

        private bool GoodStartTime(EditExceptionDto av, DateTime d)
        {

            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30);
        }

        private bool GoodEndTime(EditExceptionDto av, DateTime d)
        {
            return d.Hour >= 9 && d.Hour <= 17 && d.Second == 0 && (d.Minute == 0 || d.Minute == 30) ||
                   d.Hour >= 9 && d.Hour == 18 && d.Second == 0 && d.Minute == 0;
        }

        private bool ValidStatus(EditExceptionDto av, int status)
        {
            if (status != 0 && status != 1)
            {
                return false;
            }
            return true;
        }
    }
}
