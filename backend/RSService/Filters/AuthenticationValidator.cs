using FluentValidation;
using RSRepository;
using RSService.BusinessLogic;
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
        private IRSManager rsManager;

        public AuthenticationValidator(IRSManager rsManager)
        {
            this.rsManager = rsManager;

            RuleFor(m => m.Name).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyUsername);
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyPassword);
            RuleFor(m => m.Name).Must(IsActive).WithMessage(x => Validation.AuthMessages.IsNotActive);
          //  RuleFor(m => m.Name).Must(UserExists).WithMessage(x => Validation.AuthMessages.IsNotActive);

        }

        private bool IsActive(CredentialModel cModel, String username)
        {
            return rsManager.IsActiveUser(username);
        }

        //private bool UserExists(CredentialModel cModel, String username)
        //{
        //    return rsManager.IsUser(username);
        //}






    }
}
