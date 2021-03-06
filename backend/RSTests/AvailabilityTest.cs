using RSService.BusinessLogic;
using RSService.Validators;
using RSService.Validation;
using System;
using Xunit;
using System.Linq;
using RSService.DTO;
using System.Collections.Generic;
using RSRepository;
using System.Threading.Tasks;
using RSData.Models;
using RSService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RSTests
{
    public class AvailabilityTest
    {

        [Fact]
        public void WhenFields_AreNotFullfield_DenyAdd()
        {
            var roomMoq = new Moq.Mock<IRoomService>(Moq.MockBehavior.Strict);
            var availabilityMoq = new Moq.Mock<IAvailabilityService>(Moq.MockBehavior.Strict);
            availabilityMoq.Setup(li => li.IsGoodStartTime(Moq.It.IsAny<AddAvailabilityDto>())).Returns(true);
            availabilityMoq.Setup(li => li.IsGoodEndTime(Moq.It.IsAny<AddAvailabilityDto>())).Returns(true);
            availabilityMoq.Setup(li => li.ValidDays(Moq.It.IsAny<AddAvailabilityDto>())).Returns(true);
            availabilityMoq.Setup(li => li.ValidOccurrence(Moq.It.IsAny<AddAvailabilityDto>())).Returns(true);

            var validator = new AddAvailabilityValidator(roomMoq.Object, availabilityMoq.Object);

            var validationResults = validator.Validate(new AddAvailabilityDto());

            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyStartDate));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyEndDate));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == AvailabilityMessages.EmptyDayOfWeek));
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

            var repoMoq = new Moq.Mock<IAvailabilityRepository>(Moq.MockBehavior.Strict);

            var availabilityService = new AvailabilityService(repoMoq.Object);

            Assert.Equal(isValidOccurrence, availabilityService.ValidOccurrence(availability));
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

            var repoMoq = new Moq.Mock<IAvailabilityRepository>(Moq.MockBehavior.Strict);
            var availabilityService = new AvailabilityService(repoMoq.Object);

            Assert.Equal(isValidStartDate, availabilityService.IsGoodStartTime(availability));
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

            var repoMoq = new Moq.Mock<IAvailabilityRepository>(Moq.MockBehavior.Strict);
            var availabilityService = new AvailabilityService(repoMoq.Object);

            Assert.Equal(isValidEndDate, availabilityService.IsGoodEndTime(availability));
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

            var repoMoq = new Moq.Mock<IAvailabilityRepository>(Moq.MockBehavior.Strict);
            var availabilityService = new AvailabilityService(repoMoq.Object);

            Assert.Equal(IsValidStartDate, availabilityService.IsGoodStartTime(exception));
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

            var repoMoq = new Moq.Mock<IAvailabilityRepository>(Moq.MockBehavior.Strict);
            var availabilityService = new AvailabilityService(repoMoq.Object);

            Assert.Equal(IsValidEndDate, availabilityService.IsGoodEndTime(exception));
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

            var repoMoq = new Moq.Mock<IAvailabilityRepository>(Moq.MockBehavior.Strict);
            var availabilityService = new AvailabilityService(repoMoq.Object);

            Assert.Equal(d.IsValid, availabilityService.ValidDays(availability));
        }



        //[Fact]
        //public void GetAvailabilities_ReturnsAViewResult_WithAListOfAvailabilities()
        //{
        //    //Arrange
        //    var contextMoq = new Moq.Mock<RoomPlannerDevContext>();

        //    var userMoq = new Moq.Mock<IUserRepository>(Moq.MockBehavior.Strict);

        //    var availabilityMoq = new Moq.Mock<IAvailabilityRepository>(Moq.MockBehavior.Strict);
        //    availabilityMoq.Setup(li => li.GetAvailabilitiesByHost(Moq.It.IsAny<int>())).Returns(GetAvailabilities());
        //    var controller = new AvailabilityController(null, null, availabilityMoq.Object);

        //    //Act
        //    var result = controller.GetAvailabilities(4, new DateTime(2018, 02, 19, 09, 0, 0));

        //    //Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<AvailabilityDto>>(viewResult.ViewData.Model);
        //    Assert.Equal(2, model.Count());
        //}

        //private List<Availability> GetAvailabilities()
        //{
        //    var avaialbilities = new List<Availability>()
        //    {
        //        new Availability(new DateTime(2018, 02, 20, 09, 0, 0), new DateTime(2018, 02, 20, 13, 0, 0), 0, 2, 4, 0),
        //        new Availability(new DateTime(2018, 02, 21, 09, 0, 0), new DateTime(2018, 02, 21, 13, 0, 0), 0, 2, 4, 0)
        //    };

        //    return avaialbilities;
        //}


    }
}
