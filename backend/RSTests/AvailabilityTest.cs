using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSService.BusinessLogic;
using RSService.Controllers;
using RSService.ViewModels;
using System;

namespace RSTests
{
    [TestClass]
    public class AvailabilityTest
    {
        IRSManager _rsManager;

        [TestMethod]
        public void AddValidAvailability()
        {
            AvailabilityViewModel availability = new AvailabilityViewModel()
            {
                StartDate = new DateTime(2018, 2, 12, 9, 0, 0),
                EndDate = new DateTime(2018, 2, 12, 13, 0, 0),
                DaysOfWeek = new int[] { 1, 2, 3, 4, 5 },
                RoomId = 5,
                Occurrence = 0
            };

            AvailabilityController AvController = new AvailabilityController(_rsManager);


            // ??
            //AvController.AddAvailability(availability, 1386);



        }

        public void AddAvailabilityWithWrongTime()
        {
            AvailabilityViewModel availability = new AvailabilityViewModel()
            {
                StartDate = new DateTime(2018, 2, 12, 9, 15, 0),
                EndDate = new DateTime(2018, 2, 12, 13, 0, 0),
                DaysOfWeek = new int[] { 1, 2, 3, 4, 5 },
                RoomId = 5,
                Occurrence = 0
            };
        }


    }
}
