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
    public class RoomTests
    {

        [Fact]
        public void WhenFields_AreNotFullfield_DenyAdd()
        {
            var rsMoq = new Moq.Mock<IRoomService>(Moq.MockBehavior.Loose);

            var validator = new EditRoomValidator(rsMoq.Object);

            var validationResults = validator.Validate(new RoomDto());

            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == RoomMessages.EmptyRoomLocation));
            Assert.Equal(1, validationResults.Errors.Count(li => li.ErrorMessage == RoomMessages.EmptyRoomName));
        }


        // the maximum lenght right now is 30 for both RoomName and locationName

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
        [InlineData("tower towers ", true)]
        public void WhenLocationName_IsTooLong_DenyAdd(string locationname, bool IsValidName)
        {
            RoomDto room = new RoomDto()
            {
              Location=locationname
            };

            var rsMoq = new Moq.Mock<IRoomService>(Moq.MockBehavior.Loose);

            var validator = new EditRoomValidator(rsMoq.Object);

            var validationResults = validator.Validate(room);

            Assert.Equal(IsValidName, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == RoomMessages.LocationNameLong) == null);
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",false)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
        [InlineData("tower room ", true)]
        public void WhenRoomName_IsTooLong_DenyAdd(string roomname, bool IsValidName)
        {
            RoomDto room = new RoomDto()
            {
                Name = roomname               
            };

            var rsMoq = new Moq.Mock<IRoomService>(Moq.MockBehavior.Loose);

            var validator = new EditRoomValidator(rsMoq.Object);

            var validationResults = validator.Validate(room);

            Assert.Equal(IsValidName, validationResults.Errors.SingleOrDefault(li => li.ErrorMessage == RoomMessages.RoomNameLong) == null);
        }

    }
}
