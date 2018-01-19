using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSData.Models;
using RSRepository;
using RSService.BusinessLogic;
using RSService.DTO;
using RSService.Validation;
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
                    Id = availability.Id,
                    StartDate = availability.StartDate,
                    EndDate = availability.EndDate,
                    AvailabilityType = availability.AvailabilityType,
                    RoomId = availability.RoomId,
                    Host = availability.Host.FirstName + " " + availability.Host.LastName
                });
            }

            return Ok(availabilities);
        }

        [HttpPost("/availability/add")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult AddAvailability([FromBody] AvailabilityDto availability)
        {
            // Verifica daca exista availability pt acest host
               //- daca da: sterge si adauga unul nou
               //- daca nu: adauga

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





        

    }
}
