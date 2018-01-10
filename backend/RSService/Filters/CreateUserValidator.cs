using FluentValidation;
using RSData.Models;
using RSService.BusinessLogic;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{

    public class CreateUserValidator : AbstractValidator<UserViewModel>
    {
        private IRSManager rsManager;
        public CreateUserValidator(IRSManager rSManager)
        {
            rsManager = rSManager;

            RuleFor(m => m.Email).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyEmail);
            RuleFor(m => m.Email).Must(EmailDomain).WithMessage(x => Validation.UserMessages.EmptyEmail);
            RuleFor(m => m.FirstName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyFirstName);
            RuleFor(m => m.LastName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyLastName);
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyPassword);    
            RuleFor(m => m.Password).MinimumLength(6).WithMessage(x => Validation.UserMessages.WeakPassword);

            RuleFor(m => m.UserRole).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyUserRole);
            RuleFor(m => m.UserRole).Must(IsValidRole).WithMessage(x => Validation.UserMessages.UserRoleNotFound);

            When(m => m.Email != null && m.Email.Length != 0, () => {
                RuleFor(m => m.Email).Must(IsUniqueEmail).WithMessage(x => Validation.UserMessages.UniqueEmail);
            });
        }

        private bool IsUniqueEmail(UserViewModel m, String email)
        {
            return rsManager.IsUniqueEmail(email);
        }

        private bool EmailDomain(UserViewModel m , String email)
        {
            if (email.Contains("@stefanini.com"))
                return true;
            return false;
        }

        private bool IsValidRole(UserViewModel usm, List<int> userRole)
        {
            return rsManager.IsValidRole(userRole);  
        }

    }
}
