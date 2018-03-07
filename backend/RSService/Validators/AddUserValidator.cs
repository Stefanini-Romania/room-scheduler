﻿using FluentValidation;
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
    public class AddUserValidator : AbstractValidator<AddUserDto>
    {
        IUserService _userService;

        public AddUserValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(m => m.Email).Must(EmailFormat).WithMessage(x => Validation.UserMessages.EmailTypeWrong);
            RuleFor(m => m.Email).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyEmail);
            RuleFor(m => m.FirstName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyFirstName);
            RuleFor(m => m.LastName).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyLastName);
            RuleFor(m => m.Password).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyPassword);
            RuleFor(m => m.Password).Must(IsGoodPassword).WithMessage(x => Validation.UserMessages.WeakPassword);

            RuleFor(m => m.UserRole).NotEmpty().WithMessage(x => Validation.UserMessages.EmptyUserRole);
            RuleFor(m => m.UserRole).Must(IsValidRole).WithMessage(x => Validation.UserMessages.UserRoleNotFound);

            When(m => m.Email != null && m.Email.Length != 0, () => {
                RuleFor(m => m.Email).Must(IsUniqueEmail).WithMessage(x => Validation.UserMessages.UniqueEmail);
            });

        }

        private bool IsUniqueEmail(String email)
        {
            return _userService.IsUniqueEmail(email);
        }

        private bool IsGoodPassword(String password)
        {
            return _userService.WeakPassword(password);
        }

        private bool EmailFormat(String email)
        {
            return _userService.GoodEmailFormat(email);
        }

        private bool IsValidRole(List<int> userRole)
        {
            return _userService.IsValidRole(userRole);
        }
    }
}
