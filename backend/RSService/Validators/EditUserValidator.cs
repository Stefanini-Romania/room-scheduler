using FluentValidation;
using RSData.Models;
using RSService.BusinessLogic;
using RSService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RSService.Validators
{
    public class EditUserValidator : AbstractValidator<EditUserDto>
    {
        private IUserService _userService;
        public EditUserValidator( IUserService userService)
        {
            _userService = userService;
           
            RuleFor(m => m.Email).Must(EmailFormat).WithMessage(x => Validation.UserMessages.EmailTypeWrong);
          
            RuleFor(m => m.Email).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyEmail);

            RuleFor(m => m.Email).Must(IsUniqueEmail).WithMessage(x => Validation.UserMessages.UniqueEmail);

            RuleFor(m => m.FirstName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyFirstName);

            RuleFor(m => m.LastName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyLastName);

            //RuleFor(m => m.UserRole).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyUserRole);
            //RuleFor(m => m.UserRole).Must(IsValidRole).WithMessage(x => Validation.UserMessages.UserRoleNotFound);
           
        }
      

        private bool IsUniqueEmail(EditUserDto m, String email)
        {
            return _userService.IsUniqueEmailEdit(email, m.Id);
        }

        private bool EmailFormat(EditUserDto m, String email)
        {
            return _userService.GoodEmailFormat(email);
        }

        private bool IsValidRole(EditUserDto usm, List<int> userRole)
        {
            return _userService.IsValidRole(userRole);
        }
    }
}
