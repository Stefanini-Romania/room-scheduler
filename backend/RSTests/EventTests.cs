﻿using RSService.BusinessLogic;
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
    public class EventTests
    {

        [Fact]
        public void WhenFields_AreNotFullfield_DenyAdd()
        {
            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new CreateEventValidator(rsMoq.Object);

            var validationResults = validator.Validate(new AddEventDto());

            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == EventMessages.EmptyStartDate));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == EventMessages.EmptyEndDate));
        }

        [Theory]
        [InlineData(2018, 02, 19, 09, 0, 0, true)]
        [InlineData(2018, 02, 19, 08, 30, 0, false)]
        [InlineData(2018, 02, 19, 18, 0, 0, false)]
        [InlineData(2018, 02, 19, 13, 15, 0, false)]
        [InlineData(2018, 02, 19, 13, 0, 30, false)]
        [InlineData(2018, 02, 19, 18, 30, 0, false)]
        public void WhenStartDate_IsInBusinessHours_AllowAdding(int year, int month, int day, int hour, int minute, int second, bool isValidStartDate)
        {
            AddEventDto appointment = new AddEventDto()
            {
                StartDate = new DateTime(year, month, day, hour, minute, second)
            };

            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new CreateEventValidator(rsMoq.Object);

            var validationResults = validator.Validate(appointment);

            Assert.Equal(isValidStartDate, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == EventMessages.StartDateSpecific) == null);

        }




    }
}
