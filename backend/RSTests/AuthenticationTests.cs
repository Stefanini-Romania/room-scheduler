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
    public class AuthenticationTests
    {
        [Fact]
        public void WhenFields_AreNotFullfield_DenyLogIn()
        {
            var rsMoq = new Moq.Mock<IUserService>(Moq.MockBehavior.Strict);
            rsMoq.Setup(li => li.IsActiveUser(Moq.It.IsAny<string>())).Returns(true);

            var validator = new AuthenticationValidator(rsMoq.Object);

            var validationResults = validator.Validate(new CredentialsDto());

            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AuthMessages.EmptyEmail));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AuthMessages.EmptyPassword));
        }

    }
}
