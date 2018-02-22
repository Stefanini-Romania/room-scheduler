using RSService.BusinessLogic;
using RSService.Validators;
using RSService.Validation;
using System;
using Xunit;
using System.Linq;
using RSService.DTO;
using System.Collections.Generic;
using RSRepository;

namespace RSTests
{
    public class AvailabilityTest
    {

        [Fact]
        public void WhenFields_AreNotFullfield_DenyAdd()
        {
            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(new AddAvailabilityDto());

            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyStartDate));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyEndDate));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyDayOfWeek));
        }

        [Fact]
        public void WhenRoomId_IsEmpty_NoDatabaseCallIsMade()
        {
            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);
            rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Throws(new Exception("IsActiveRoom is called"));

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(new AddAvailabilityDto());
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        [InlineData(-1, false)]
        public void WhenOccurence_IsInGoodRange_AllowAdding(int occurence, bool isValidOccurrence)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                Occurrence = occurence,
            };


            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);
            // Facem setup-ul pt a face validarea de active room sa treaca, intrucat testam doar occurrence-ul
            //rsMoq.Setup(li => li.IsActiveRoom(Moq.It.IsAny<int>())).Returns(true);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);

            Assert.Equal(isValidOccurrence, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectOccurrence) == null);
        }

        [Theory]
        [InlineData(2018, 02, 19, 10, 0, 0, true)]
        [InlineData(2018, 02, 19, 17, 30, 0, true)]
        [InlineData(2018, 02, 19, 10, 15, 0, false)]
        [InlineData(2018, 02, 19, 10, 0, 30, false)]
        [InlineData(2018, 02, 19, 08, 0, 0, false)]
        [InlineData(2018, 02, 19, 18, 0, 0, false)]
        public void WhenStartDate_IsInBusinessHours_AllowAdding(int year, int month, int day, int hour, int minute, int second, bool isValidStartDate)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                StartDate = new DateTime(year, month, day, hour, minute, second)
            };

            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);

            Assert.Equal(isValidStartDate, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectStartTime) == null);
        }

        [Theory]
        [InlineData(2018, 02, 19, 13, 0, 0, true)]
        [InlineData(2018, 02, 19, 18, 0, 0, true)]
        [InlineData(2018, 02, 19, 13, 15, 0, false)]
        [InlineData(2018, 02, 19, 13, 0, 30, false)]
        [InlineData(2018, 02, 19, 18, 30, 0, false)]
        public void WhenEndDate_IsInBusinessHours_AllowAdding(int year, int month, int day, int hour, int minute, int second, bool isValidEndDate)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                EndDate = new DateTime(year, month, day, hour, minute, second)
            };

            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);

            Assert.Equal(isValidEndDate, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectEndTime) == null);
        }
        [Theory]
        [InlineData(2018, 02, 20, 10, 0, 0, true)]
        [InlineData(2018, 02, 19, 17, 0, 0, true)]
        [InlineData(2018, 02, 19, 08, 00, 0, false)]
        [InlineData(2018, 02, 19, 10, 25, 0, false)]
        [InlineData(2018, 02, 19, 09, 30, 0, true)]
        public void WhenExceptionStartTime_InBusinessHours_AllowAdding(int year , int month, int day, int hour, int minute, int second, bool IsValidStartDate)
        {
            AvailabilityExceptionDto exception = new AvailabilityExceptionDto()
            {
                StartDate = new DateTime(year, month, day, hour, minute, second)
            };

            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new AddExceptionValidator(rsMoq.Object);

            var validationResult = validator.Validate(exception);

            Assert.Equal(IsValidStartDate, validationResult.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectStartTime) == null);


        }

        [Theory]
        [InlineData(2018, 02, 20, 10, 30, 0, true)]
        [InlineData(2018, 02, 20, 08, 30, 0, false)]
        [InlineData(2018, 02, 20, 09, 25, 0, false)]
        [InlineData(2018, 02, 20, 18, 30, 0, false)]
        [InlineData(2018, 02, 20, 13, 30, 0, true)]
        [InlineData(2018, 02, 20, 09, 0, 0, true)]
        public void WhenExceptionEndTime_InBusinessHours_Allowadding(int year, int month, int day, int hour, int minute, int second, bool IsValidEndDate)
        {
            AvailabilityExceptionDto exception = new AvailabilityExceptionDto()
            {
                EndDate = new DateTime(year, month, day, hour, minute, second)

            };

            var rsmoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new AddExceptionValidator(rsmoq.Object);

            var validationResult = validator.Validate(exception);

            Assert.Equal(IsValidEndDate, validationResult.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectEndTime) == null);
        }

        public class DaysOfWeek
        {
            public int[] Days { get; set; }
            public bool IsValid { get; set; }
        }

        public static IEnumerable<object[]> GetDays()
        {
            yield return new object[] {
                new DaysOfWeek() { Days = new int[]{ 1, 2, 3 }, IsValid = true }
            };
            yield return new object[]
            {
                new DaysOfWeek() { Days = new int[] { 1, 3, 6 }, IsValid = false }
            };
            yield return new object[]
            {
                new DaysOfWeek() { Days = new int[]{ 1, 2, 3, 4, 5, 1 }, IsValid = false }
            };
        }

        [Theory]
        [MemberData(nameof(GetDays))]
        public void WhenDaysOfWeek_IsInGoodRange_AllowAdding(DaysOfWeek d)
        {
            AddAvailabilityDto availability = new AddAvailabilityDto()
            {
                DaysOfWeek = d.Days
            };

            var rsMoq = new Moq.Mock<IRSBusiness>(Moq.MockBehavior.Strict);

            var validator = new AddAvailabilityValidator(rsMoq.Object);

            var validationResults = validator.Validate(availability);
            Assert.Equal(d.IsValid, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == AvailabilityMessages.IncorrectDayOfWeek) == null);
        }

        [Fact]
        public void GetAvailabilities_ReturnsAViewResult_WithAListOfAvailabilities()
        {
            //Arrange
            var mockRepo = new Moq.Mock<RoomPlannerDevContext>();
            

            //mockRepo.Setup(repo => repo.ListAsync()).Returns(Task.FromResult(GetTestSessions()));
            //var controller = new HomeController(mockRepo.Object);

            //Act

            //Assert
        }


    }
}
