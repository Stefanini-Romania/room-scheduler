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
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == UserMessages.EmptyUserRole));

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

        [Theory]
        [InlineData("alibaba@jaja.com",true)]
        [InlineData("alibaba@.com", false)]
        [InlineData("alibaba@com", false)]
        [InlineData("alib@comaba@com", false)]
        [InlineData("johnjohn.com@", false)]
        [InlineData("@.com", false)]
        [InlineData("alibaba@yahoo.gmail.com", true)]
        [InlineData("alina.com@johnny.com", true)]
        [InlineData("havier@com", false)]
        public void WhenEmail_HasWrongFormat_DenyAdd(string email, bool isValidEmail)
        {
            AddUserDto mail = new AddUserDto()
            {
                Email=email
            };

            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Loose);

            var validator = new AddUserValidator(rsMoq.Object);

            var validationResults = validator.Validate(mail);

            Assert.Equal(isValidEmail, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == UserMessages.EmailTypeWrong) == null);
        }
    }
}
