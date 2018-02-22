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
    public class EventTests
    {

        [Fact]
        public void WhenFields_AreNotFullfield_DenyAdd()
        {
            var eventMoq = new Moq.Mock<IEventService>(Moq.MockBehavior.Strict);

            var penaltyMoq = new Moq.Mock<IPenaltyService>(Moq.MockBehavior.Strict);           

            var validator = new CreateEventValidator(eventMoq.Object, penaltyMoq.Object);

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

            var eventMoq = new Moq.Mock<IEventService>(Moq.MockBehavior.Strict);

            var penaltyMoq = new Moq.Mock<IPenaltyService>(Moq.MockBehavior.Strict);

            var validator = new CreateEventValidator(eventMoq.Object, penaltyMoq.Object);

            var validationResults = validator.Validate(appointment);

            Assert.Equal(isValidStartDate, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == EventMessages.StartDateSpecific) == null);

        }

        [Theory]
        [InlineData(2018, 02, 21, 09, 30, 0, true)]
        [InlineData(2018, 02, 24, 09, 30, 0, false)]
        [InlineData(2018, 02, 25, 17, 30, 0, false)]
        public void WhenStartDate_IsInWeekendDays_AllowAdding(int year, int month, int day, int hour, int minute, int second, bool isValidStartDate)
        {
            AddEventDto appotment = new AddEventDto()
            {
                StartDate = new DateTime(year, month, day, hour, minute,second)
            };

            var eventMoq = new Moq.Mock<IEventService>(Moq.MockBehavior.Strict);

            var penaltyMoq = new Moq.Mock<IPenaltyService>(Moq.MockBehavior.Strict);

            var validator = new CreateEventValidator(eventMoq.Object, penaltyMoq.Object);

            var validationResult = validator.Validate(appotment);

            Assert.Equal(isValidStartDate, validationResult.Errors.SingleOrDefault(li => li.ErrorMessage == EventMessages.DayOfWeekWeekend) == null);

        }


    }
}
