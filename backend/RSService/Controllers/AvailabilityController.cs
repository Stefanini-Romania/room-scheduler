using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.BusinessLogic;
using RSService.DTO;
using RSService.Validation;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Controllers
{
    public class AvailabilityController : BaseController
    {
        private IRSManager rsManager;
        private IUserRepository userRepository;
        private IAvailabilityRepository availabilityRepository;

        public AvailabilityController(IRSManager rsManager)
        {
            this.rsManager = rsManager;
            userRepository = new UserRepository(Context);
            availabilityRepository = new AvailabilityRepository(Context);
        }

        [HttpGet("/availability/list")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult GetAvailabilities()
        {
            var results = availabilityRepository.GetAvailabilities();
            if (results == null) return NotFound();

            List<AvailabilityDto> availabilities = new List<AvailabilityDto>();
            foreach(var availability in results)
            {
                availabilities.Add(new AvailabilityDto()
                {
                    StartDate = availability.StartDate,
                    EndDate = availability.EndDate,
                    AvailabilityType = availability.AvailabilityType,
                    RoomId = availability.RoomId,
                    HostName = availability.Host.FirstName + " " + availability.Host.LastName,
                    HostId = availability.HostId

                });
            }

            return Ok(availabilities);
        }

        [HttpPost("/availability/add")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult AddAvailability([FromBody] IEnumerable<AvailabilityViewModel> newAvailability)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Availability);
            }

            // If already exists availabilities for this host, remove them
            var dbAvailabilities = availabilityRepository.GetAvailabilities(newAvailability.First().AvailabilityType, newAvailability.First().RoomId, newAvailability.First().HostId);
            if (dbAvailabilities != null)
            {
                availabilityRepository.RemoveAvailabilities(dbAvailabilities);
            }

            int dayOfWeek = 0;

            foreach(var av in newAvailability)
            {
                dayOfWeek++;
                Availability availability = new Availability(
                av.StartDate,
                av.EndDate,
                dayOfWeek,
                av.AvailabilityType,
                av.RoomId,
                av.HostId
                );
                availabilityRepository.AddAvailability(availability);
            }
            Context.SaveChanges();

            return Ok();
        }


        [HttpPost("/availability/exception/add")]
        //[Authorize(Roles = nameof(UserRoleEnum.admin) +","+ nameof(UserRoleEnum.host))]
        [Authorize(Roles = nameof(UserRoleEnum.host))]
        public IActionResult AddException([FromBody] AvailabilityExceptionDto avException)
        {
            if (!ModelState.IsValid)
                return ValidationError(GeneralMessages.Availability);

            var schedulerIdentity = SchedulerIdentity.Current(HttpContext);
            var attendee = userRepository.GetUserById(schedulerIdentity.UserId);
            if (attendee.IsActive != true)
            {
                return ValidationError(EventMessages.InactiveUser);
            }

            Availability availability = new Availability(
                                        avException.StartDate, 
                                        avException.EndDate, 
                                        (int)AvailabilityEnum.Exception,
                                        null,
                                        attendee.Id
                                        );

            availabilityRepository.AddAvailability(availability);
            Context.SaveChanges();

            return Ok();
        }

        [HttpPost("/availability/remove/{roomId}/{hostId}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult RemoveAvailabilities(int roomId, int hostId)
        {

            var dbAvailabilities = availabilityRepository.GetAvailabilities((int)AvailabilityEnum.Available, roomId, hostId);
            if (dbAvailabilities != null)
            {
                availabilityRepository.RemoveAvailabilities(dbAvailabilities);
            }

            Context.SaveChanges();

            return Ok();
        }





    }
}
