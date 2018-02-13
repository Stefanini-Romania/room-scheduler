using FluentValidation;
using RSService.BusinessLogic;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class EditParameterValidator : AbstractValidator<SettingsDto>
    {
        IRSManager rsManager;

        public EditParameterValidator(IRSManager _rsManager)
        {
            rsManager = _rsManager;

            When(p => p.VarName == "SessionTimeSpan", () =>
            {
                RuleFor(p => p.Value).Must(IsNumber).WithMessage(p => Validation.SettingsMessages.WrongValue);
            });

            When(p => p.VarName == "EmailReminderTime", () =>
            {
                RuleFor(p => p.Value).Must(IsNumber).WithMessage(p => Validation.SettingsMessages.WrongValue);
            });
        }


        private bool IsNumber(SettingsDto settings, string value)
        {
            double number;

            if (Double.TryParse(value, out number))
            {
                return true;
            }
            return false;
        }

    }
}
