using RSService.BusinessLogic;
using RSService.DTO;
using RSService.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using RSService.Validation;

namespace RSTests
{
    public class UserTests
    {

        [Fact]
        public void WhenFields_AreNotFullfield_DenyAdd()
        {
            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Loose);

            var validator = new AddUserValidator(rsMoq.Object);

            var validationResults = validator.Validate(new AddUserDto());

            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == UserMessages.EmptyEmail));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == UserMessages.EmptyFirstName));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == UserMessages.EmptyLastName));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == UserMessages.EmptyPassword));

        }

        [Theory]
        [InlineData("1234",false)]
        [InlineData("asdfg", false)]
        [InlineData("123456", true)]
        [InlineData("1234567890568464", true)]
        public void WhenPassword_IsWeak_DenyAdd(string pass, bool IsValidPassowrd)
        {
            AddUserDto user = new AddUserDto()
            {
                Password = pass
            };

            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Loose);

            var validator = new AddUserValidator(rsMoq.Object);

            var validationResults = validator.Validate(user);

            Assert.Equal(IsValidPassowrd, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == UserMessages.WeakPassword) == null);
        
        }
    }
}
