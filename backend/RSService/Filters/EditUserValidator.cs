using FluentValidation;
using RSService.BusinessLogic;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Filters
{
    public class EditUserValidator : AbstractValidator<EditUserViewModel>
    {
        private IRSManager rsManager;
        public EditUserValidator(IRSManager rSManager)
        {
            rsManager = rSManager;

            RuleFor(m => m.Name).Must(IsUniqueUserName).WithMessage(x => Validation.UserMessages.UniqueUsername);

            RuleFor(m => m.Name).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyUsername);

            RuleFor(m => m.Email).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyEmail);

            RuleFor(m => m.Email).Must(IsUniqueEmail).WithMessage(x => Validation.UserMessages.UniqueEmail);

            RuleFor(m => m.FirstName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyFirstName);

            RuleFor(m => m.LastName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyLastName);

            //RuleFor(m => m.UserRole).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyUserRole);
            //RuleFor(m => m.UserRole).Must(IsValidRole).WithMessage(x => Validation.UserMessages.UserRoleNotFound);

             

        }

        private bool IsUniqueUserName(EditUserViewModel m, String userName)
        {
            return rsManager.IsUniqueUserNameEdit(userName, m.Id);
        }

        private bool IsUniqueEmail(EditUserViewModel m, String email)
        {
            return rsManager.IsUniqueEmail(email, m.Id);
        }

        private bool IsValidRole(EditUserViewModel usm, List<int> userRole)
        {
            return rsManager.IsValidRole(userRole);
        }
    }
}
