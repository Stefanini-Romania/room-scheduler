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
        private IRSBusiness rsBusiness;

        public AuthenticationValidator(IRSBusiness rsBusiness)
        {
            this.rsBusiness = rsBusiness;

            RuleFor(m => m.LoginName).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyEmail);
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyPassword);
            RuleFor(m => m.LoginName).Must(IsActive).WithMessage(x => Validation.AuthMessages.IsNotActive);

        }

        private bool IsActive(CredentialsDto cModel, String email)
        {
            return rsBusiness.IsActiveUser(email);
        }

    }
}
