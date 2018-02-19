using FluentValidation;
using RSService.BusinessLogic;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        
        public ResetPasswordValidator()
        {
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyPassword);
            RuleFor(m => m.Password).MinimumLength(6).WithMessage(x => Validation.UserMessages.WeakPassword);
        }
    }
}
