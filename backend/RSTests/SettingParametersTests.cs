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
        [InlineData("SessionTimeSpan", "23", true)]
        [InlineData("SessionTimeSpan", "abc", false)]
        public void WhenData_IsNumber_AllowEdit(string name,string value, bool IsValidData)
        {
            SettingsDto parameters = new SettingsDto()
            {
                VarName = name,
                Value = value
            };

            var settingsParameterService = new SettingsParameterService();

            Assert.Equal(IsValidData, settingsParameterService.IsNumber(parameters.Value));
        }


        [Theory]
        [InlineData("SessionTimeSpan","100",false)]
        [InlineData("SessionTimeSpan","30",true)]
        public void WhenValue_IsInGoodRange_AllowEdit(string name ,string value, bool IsValidData)
        {
            SettingsDto valaore = new SettingsDto()
            {
                VarName=name,
                Value = value
            };

            var settingsParameterService = new SettingsParameterService();

            Assert.Equal(IsValidData, settingsParameterService.IsGoodSessionTime(value));
        }
    }
}
