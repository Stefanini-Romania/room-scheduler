using RSService.BusinessLogic;
using RSService.DTO;
using RSService.Validators;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace RSTests
{
    public class SettingParametersTests
    {

        [Theory]
        [InlineData("SessionTimeSpan","23",true)]
        [InlineData("SessionTimeSpan","20",true)]
        public void WhenData_IsNotNumber_DenyEdit(string name,string value, bool IsValidData)
        {
            SettingsDto parameters = new SettingsDto()
            {
                VarName = name,
                Value = value
            };
            var validator = new EditParameterValidator();

            var validationResults = validator.Validate(parameters);

            Assert.Equal(IsValidData, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == SettingsMessages.WrongValue)==null);


        }


        [Theory]
        [InlineData("SessionTimeSpan","10000",false)]
        [InlineData("SessionTimeSpan","30",true)]
        public void WhenValue_IsTooBig_DenyEdit(string name ,string value, bool IsValidData)
        {
            SettingsDto valaore = new SettingsDto()
            {
                VarName=name,
                Value = value
            };
            var validator = new EditParameterValidator();

            var validationResults = validator.Validate(valaore);

            Assert.Equal(IsValidData, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == SettingsMessages.SessionValueTooSmallOrTooBig) == null);
        }
    }
}
