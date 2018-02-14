using RSService.BusinessLogic;
using RSService.Filters;
using RSService.Validation;
using System;
using Xunit;
using System.Linq;
using RSService.DTO;

namespace RSTests
{
    public class AvailabilityTest
    {
        [Fact]
        public void WhenFields_AreNotFullfield_DenyAdd()
        {
            // Folosim moq pt a face validarea de active room sa treaca, intrucat testam altceva

            var rsMoq = new Moq.Mock<IRSManager>(Moq.MockBehavior.Strict);
            rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Returns(false);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(new AddAvailabilityDto());

            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyStartDate));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyEndDate));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyRoomId));
        }

        [Fact]
        public void WhenRoomId_IsEmpty_NoDatabaseCallIsMade()
        {
            var rsMoq = new Moq.Mock<IRSManager>(Moq.MockBehavior.Strict);
            rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Throws(new Exception("IsActiveRoom is called"));

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(new AddAvailabilityDto());
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        [InlineData(-1, false)]
        public void WhenOccurence_Varies_IsAsExpected(int occurence, bool isValidOccurrence)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                Occurrence = occurence,
            };

            // Folosim moq pt a face validarea de active room sa treaca, intrucat testam doar occurrence-ul
            var rsMoq = new Moq.Mock<IRSManager>(Moq.MockBehavior.Strict);
            rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Returns(true);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);

            Assert.Equal(isValidOccurrence, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectOccurrence) == null);
        }

        [Theory]
        [InlineData(2018, 02, 19, 10, 0, 0, true)]
        [InlineData(2018, 02, 19, 10, 15, 0, false)]
        [InlineData(2018, 02, 19, 10, 0, 30, false)]
        [InlineData(2018, 02, 19, 08, 0, 0, false)]
        [InlineData(2018, 02, 19, 18, 0, 0, false)]
        [InlineData(2018, 02, 19, 17, 30, 0, true)]
        public void WhenStartDate_IsInBusinessHours_AllowAdding(int year, int month, int day, int hour, int minute, int second, bool isValidStartDate)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                StartDate = new DateTime(year, month, day, hour, minute, second)
            };

            var rsMoq = new Moq.Mock<IRSManager>(Moq.MockBehavior.Strict);
            rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Returns(true);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);

            Assert.Equal(isValidStartDate, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectStartTime) == null);
        }

        [Theory]
        [InlineData(2018, 02, 19, 10, 0, 0, true)]
        [InlineData(2018, 02, 19, 10, 15, 0, false)]
        [InlineData(2018, 02, 19, 10, 0, 30, false)]
        [InlineData(2018, 02, 19, 08, 0, 0, false)]
        [InlineData(2018, 02, 19, 18, 0, 0, false)]
        [InlineData(2018, 02, 19, 17, 30, 0, true)]
        public void WhenEndDate_IsInBusinessHours_AllowAdding(int year, int month, int day, int hour, int minute, int second, bool isValidEndDate)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                EndDate = new DateTime(year, month, day, hour, minute, second)
            };

            var rsMoq = new Moq.Mock<IRSManager>(Moq.MockBehavior.Strict);
            rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Returns(true);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);

            Assert.Equal(isValidEndDate, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectEndTime) == null);
        }





    }
}
