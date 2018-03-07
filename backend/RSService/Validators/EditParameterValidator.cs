using FluentValidation;
using RSService.BusinessLogic;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class EditParameterValidator : AbstractValidator<SettingsDto>
    {
        ISettingsParameterService _settingsParameterService;

        public EditParameterValidator(ISettingsParameterService settingsParameterService)
        {
            _settingsParameterService = settingsParameterService;

            When(p => p.VarName == "SessionTimeSpan", () =>
            {
                RuleFor(p => p.Value).Must(IsNumber).WithMessage(p => Validation.SettingsMessages.WrongValue);
                RuleFor(p => p.Value).Must(IsGoodSessionTime).WithMessage(p => Validation.SettingsMessages.SessionValueTooSmallOrTooBig);
            });

            When(p => p.VarName == "EmailReminderTime", () =>
            {
                RuleFor(p => p.Value).Must(IsNumber).WithMessage(p => Validation.SettingsMessages.WrongValue);
                RuleFor(p => p.Value).Must(IsGoodReminderTime).WithMessage(p => Validation.SettingsMessages.EmailValueTooSmallOrTooBig);
            });
        
    }


        private bool IsNumber(string value)
        {
            return _settingsParameterService.IsNumber(value);
        }

        private bool IsGoodReminderTime(string value)
        {
            return _settingsParameterService.IsGoodReminderTime(value);
        }

        private bool IsGoodSessionTime(string value)
        {
            return _settingsParameterService.IsGoodSessionTime(value);
        }

    }
}
