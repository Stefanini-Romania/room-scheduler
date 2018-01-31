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
        public IActionResult GetAvailabilities(int? hostId, DateTime startDate)
        {
            if (startDate == DateTime.MinValue)
            {
                return BadRequest();
            }

            if (!hostId.HasValue)
            {
                var exceptions = availabilityRepository.GetAvailabilitiesByType((int)AvailabilityEnum.Exception, startDate, startDate.AddDays(5));

                List<AvailabilityDto> finalResults = new List<AvailabilityDto>();
                foreach(var ex in exceptions)
                {
                    finalResults.Add(new AvailabilityDto(ex.StartDate, ex.EndDate, ex.DayOfWeek, ex.AvailabilityType));
                }
                return Ok(finalResults);
            }

            var availabilities = availabilityRepository.GetAvailabilitiesByHost((int)hostId);
            if (availabilities == null) return NotFound();

            List<AvailabilityDto> results = new List<AvailabilityDto>();

            foreach(var av in availabilities)
            {
                if (av.AvailabilityType == (int)AvailabilityEnum.Exception)
                {
                    if (av.StartDate >= startDate && av.StartDate <= startDate.AddDays(4))
                    {
                        results.Add(new AvailabilityDto(av.StartDate, av.EndDate, av.DayOfWeek, av.AvailabilityType));
                    }
                }
                else
                {
                    DateTime date = av.StartDate;
                    while (date.Date <= startDate.Date)
                    {
                        if (date.Date == startDate.Date)
                        {
                            results.Add(new AvailabilityDto(
                                            date, 
                                            new DateTime(date.Year,date.Month,date.Day, av.EndDate.Hour, av.EndDate.Minute, av.EndDate.Second), 
                                            av.DayOfWeek, 
                                            av.AvailabilityType, 
                                            av.RoomId));
                        }
                        date = date.AddDays(7 * (int)av.Occurrence);
                    }
                }
            }
            return Ok(results);
        }

        [HttpGet("/availability/host/list")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult GetHosts()
        {
            var results = userRepository.GetHosts();
            if (results == null) return NotFound();

            List<HostDto> final_result = new List<HostDto>();

            foreach (var it in results)
            {
                final_result.Add(new HostDto()
                {
                    Id = it.Id,
                    FirstName = it.FirstName,
                    LastName = it.LastName
                });
            }
            return Ok(final_result);
        }

        [HttpPost("/availability/add")]
        [Authorize(Roles = nameof(UserRoleEnum.admin) + "," + nameof(UserRoleEnum.host))]
        public IActionResult AddAvailability([FromBody] AvailabilityViewModel newAvailability)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Availability);
            }

            // Validations
            // If already exists availabilities for this host in this day, return Error
            //var dbAvailabilities = availabilityRepository.GetAvailabilities(newAvailability);
            //if (dbAvailabilities != null)
            //{

            //}

            foreach(var day in newAvailability.DaysOfWeek)
            {
                Availability availability = new Availability(
                    newAvailability.StartDate,
                    newAvailability.EndDate,
                    day,
                    newAvailability.AvailabilityType,
                    newAvailability.RoomId,
                    newAvailability.HostId,
                    newAvailability.Occurence
                );
                availabilityRepository.AddAvailability(availability);
            }

            Context.SaveChanges();

            return Ok();
        }


        [HttpPost("/availability/exception/add")]
        [Authorize(Roles = nameof(UserRoleEnum.admin) +","+ nameof(UserRoleEnum.host))]
        public IActionResult AddException([FromBody] AvailabilityExceptionDto avException, int? hostId)
        {
            if (!ModelState.IsValid)
                return ValidationError(GeneralMessages.Availability);

            var schedulerIdentity = SchedulerIdentity.Current(HttpContext);
            var currentUser = userRepository.GetUserById(schedulerIdentity.UserId);
            if (currentUser.IsActive != true)
            {
                return ValidationError(EventMessages.InactiveUser);
            }

            if (currentUser.UserRole.Select(li => li.RoleId).Contains((int)UserRoleEnum.host))
            {
               
                Availability availability = new Availability(
                                        avException.StartDate,
                                        avException.EndDate,
                                        (int)AvailabilityEnum.Exception,
                                        null, 
                                        currentUser.Id,
                                        null
                                        );

                availabilityRepository.AddAvailability(availability);
                
            }
            else  //Admin
            {
                if (!hostId.HasValue)
                {
                    return ValidationError(AvailabilityMessages.EmptyHostId);
                }
                Availability availability = new Availability(
                                        avException.StartDate,
                                        avException.EndDate,
                                        (int)AvailabilityEnum.Exception,
                                        null,
                                        (int)hostId,
                                        null
                                        );

                availabilityRepository.AddAvailability(availability); 
            }

            Context.SaveChanges();

            return Ok();
        }

        //[HttpPost("/availability/remove/{roomId}/{hostId}")]
        //[Authorize(Roles = nameof(UserRoleEnum.admin))]
        //public IActionResult RemoveAvailabilities(int roomId, int hostId)
        //{

        //    var dbAvailabilities = availabilityRepository.GetAvailabilities((int)AvailabilityEnum.Available, roomId, hostId);
        //    if (dbAvailabilities != null)
        //    {
        //        availabilityRepository.RemoveAvailabilities(dbAvailabilities);
        //    }

        //    Context.SaveChanges();

        //    return Ok();
        //}





    }
}
