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
            RuleFor(m => m.Name).Must(IsUniqueUserName).WithMessage(x => Validation.UserMessages.UniqueUsername);
            RuleFor(m => m.Name).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyUsername);
            RuleFor(m => m.FirstName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyFirstName);
            RuleFor(m => m.LastName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyLastName);
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyPassword);    
            RuleFor(m => m.Password).MinimumLength(6).WithMessage(x => Validation.UserMessages.WeakPassword);
            When(m => m.Email != null && m.Email.Length != 0, () => {
                RuleFor(m => m.Email).Must(IsUniqueEmail).WithMessage(x => Validation.UserMessages.UniqueEmail);
            });
        }

        public bool IsUniqueUserName(UserModel m, String userName) {
            return rSManager.IsUniqueUserName(userName);
        }

        public bool IsUniqueEmail(UserModel m, String email)
        {
            return rSManager.IsUniqueUserName(email);
        }

    }
}
