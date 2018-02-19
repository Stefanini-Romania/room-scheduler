using FluentValidation;
using RSRepository;
using RSService.BusinessLogic;
using RSService.Controllers;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class AuthenticationValidator : AbstractValidator<CredentialsDto>
    {
        private IRSManager rsManager;

        public AuthenticationValidator(IRSManager rsManager)
        {
            this.rsManager = rsManager;

            RuleFor(m => m.LoginName).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyEmail);
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyPassword);
            RuleFor(m => m.LoginName).Must(IsActive).WithMessage(x => Validation.AuthMessages.IsNotActive);

        }

        private bool IsActive(CredentialsDto cModel, String email)
        {
            return rsManager.IsActiveUser(email);
        }

    }
}
