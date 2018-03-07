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
        IUserService _userService;

        public ResetPasswordValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyPassword);
            RuleFor(m => m.Password).Must(IsGoodPassword).WithMessage(x => Validation.UserMessages.WeakPassword);
        }


        private bool IsGoodPassword(String password)
        {
            return _userService.WeakPassword(password);
        }
    }
}
