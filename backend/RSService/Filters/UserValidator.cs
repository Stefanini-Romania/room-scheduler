using FluentValidation;
using RSService.BusinessLogic;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{

    public class UserValidator : AbstractValidator<UserModel>
    {
        private IRSManager rSManager;
        public UserValidator(IRSManager rSManager)
        {
            this.rSManager = rSManager;
            RuleFor(m => m.Name).Must(IsUniqueUserName).WithMessage("");
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.AuthMessages.EmptyPassword);
        }

        public bool IsUniqueUserName(UserModel m, String userName) {
            return rSManager.IsUniqueUserName(userName);
        }

    }
}
