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
    public class EditUserValidator : AbstractValidator<EditUserDto>
    {
        private IRSBusiness _rsBusiness;
        public EditUserValidator(IRSBusiness rsBusiness)
        {
            _rsBusiness = rsBusiness;
           
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
            return _rsBusiness.IsUniqueEmailEdit(email, m.Id);
        }

        private bool EmailFormat(EditUserDto m, String email)
        {
            string MatchEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"+ "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"; ;
            if (email != null)
            {             
                return Regex.IsMatch(email, MatchEmailPattern);
            }
            return false;
        }

        private bool IsValidRole(EditUserDto usm, List<int> userRole)
        {
            return _rsBusiness.IsValidRole(userRole);
        }
    }
}