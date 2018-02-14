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

           When(p => p.VarName.Equals("SessionTimeSpan"), () =>
            {
                RuleFor(p => p.Value).Must(IsNumber).WithMessage(p => Validation.SettingsMessages.WrongValue);
                RuleFor(p => p.Value).Must(SessionMaxMinValue).WithMessage(p => Validation.SettingsMessages.SessionValueTooSmallOrTooBig);
            });

               When(p => p.VarName == "EmailReminderTime", () =>
               {
            RuleFor(p => p.Value).Must(IsNumber).WithMessage(p => Validation.SettingsMessages.WrongValue);
            RuleFor(p => p.Value).Must(EmailMaxMinValue).WithMessage(p => Validation.SettingsMessages.EmailValueTooSmallOrTooBig);
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

        private bool EmailMaxMinValue(SettingsDto settings, string value)
        {
            int valoare = Int32.Parse(value);
            if (valoare >= 10 && valoare <= 60)
                return true;
            return false;
        }
        private bool SessionMaxMinValue(SettingsDto settings, string value)
        {
            int valoare = Int32.Parse(value);
            if (valoare >= 1 && valoare <= 60)
                return true;
            return false;
        }

    }
}
