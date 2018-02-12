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
        public void WhenOccurence_Varies_IsAsExpected(int occurence, bool isOccurenceValid)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                Occurrence = occurence,
            };

            var rsMoq = new Moq.Mock<IRSManager>(Moq.MockBehavior.Loose);
            rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Returns(true);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);

            Assert.Equal(isOccurenceValid, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectOccurrence) == null);
        }
    }
}
