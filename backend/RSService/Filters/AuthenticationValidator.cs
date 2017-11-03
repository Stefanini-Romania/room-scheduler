using FluentValidation;
using RSService.Controllers;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class AuthenticationValidator : AbstractValidator<CredentialModel>
    {

        public AuthenticationValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyUsername);

            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyPassword);
        }
    }
}
