using FluentValidation;
using RSService.BusinessLogic;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordViewModel>
    {
        private IRSManager rsManager;
        public ResetPasswordValidator(IRSManager rSManager)
        {
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyPassword);
            RuleFor(m => m.Password).MinimumLength(6).WithMessage(x => Validation.UserMessages.WeakPassword);
        }
    }
}
