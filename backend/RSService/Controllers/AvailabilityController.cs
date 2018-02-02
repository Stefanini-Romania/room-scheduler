using AutoMapper;
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
                    finalResults.Add(new AvailabilityDto(ex.Id, ex.StartDate, ex.EndDate, ex.DayOfWeek, ex.AvailabilityType));
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
                        results.Add(new AvailabilityDto(av.Id, av.StartDate, av.EndDate, av.DayOfWeek, av.AvailabilityType));
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
                                            av.Id,
                                            date.AddDays(av.DayOfWeek -1), 
                                            new DateTime(date.Year, date.Month, date.Day, av.EndDate.Hour, av.EndDate.Minute, av.EndDate.Second).AddDays(av.DayOfWeek -1), 
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
        public IActionResult AddAvailability([FromBody] AvailabilityViewModel newAvailability, int? hostId)
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
            // Un host sa poata apela serviciile doar pt programul sau, nu si pt programul altui host 

            var schedulerIdentity = SchedulerIdentity.Current(HttpContext);
            var currentUser = userRepository.GetUserById(schedulerIdentity.UserId);
            if (currentUser.IsActive != true)
            {
                return ValidationError(EventMessages.InactiveUser);
            }

            if (currentUser.UserRole.Select(li => li.RoleId).Contains((int)UserRoleEnum.host))
            {
                foreach (var day in newAvailability.DaysOfWeek)
                {
                    Availability availability = new Availability(
                                        newAvailability.StartDate,
                                        newAvailability.EndDate,
                                        day,
                                        (int)AvailabilityEnum.Available,
                                        newAvailability.RoomId,
                                        currentUser.Id,
                                        newAvailability.Occurrence
                                        );

                    availabilityRepository.AddAvailability(availability);
                }
            }
            else   // Admin:
            {
                if (!hostId.HasValue)
                {
                    return ValidationError(AvailabilityMessages.EmptyHostId);
                }

                foreach (var day in newAvailability.DaysOfWeek)
                {
                    Availability availability = new Availability(
                        newAvailability.StartDate,
                        newAvailability.EndDate,
                        day,
                        newAvailability.AvailabilityType,
                        newAvailability.RoomId,
                        (int)hostId,
                        newAvailability.Occurrence
                    );
                    availabilityRepository.AddAvailability(availability);
                }
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
                //Find the rooms where the host has availability and add exception for each room
                var availabilities = availabilityRepository.GetAvailabilitiesByHostAndDay(currentUser.Id, (int)avException.StartDate.DayOfWeek);

                foreach (var av in availabilities)
                {
                    Availability availability = new Availability(
                                        new DateTime(avException.StartDate.Year, avException.StartDate.Month, avException.StartDate.Day, av.StartDate.Hour, av.StartDate.Minute, av.StartDate.Second),
                                        new DateTime(avException.EndDate.Year, avException.EndDate.Month, avException.EndDate.Day, av.EndDate.Hour,av.EndDate.Minute, av.EndDate.Second),
                                        (int)AvailabilityEnum.Exception,
                                        av.RoomId,
                                        currentUser.Id,
                                        null
                                        );
                    availabilityRepository.AddAvailability(availability);
                } 
            }
            else  //Admin
            {
                if (!hostId.HasValue)
                {
                    return ValidationError(AvailabilityMessages.EmptyHostId);
                }
                //Find the rooms where the host has availability and add exception for each room
                var availabilities = availabilityRepository.GetAvailabilitiesByHostAndDay((int)hostId, (int)avException.StartDate.DayOfWeek);

                foreach (var av in availabilities)
                {
                    Availability availability = new Availability(
                                        new DateTime(avException.StartDate.Year, avException.StartDate.Month, avException.StartDate.Day, av.StartDate.Hour, av.StartDate.Minute, av.StartDate.Second),
                                        new DateTime(avException.EndDate.Year, avException.EndDate.Month, avException.EndDate.Day, av.EndDate.Hour, av.EndDate.Minute, av.EndDate.Second),
                                        (int)AvailabilityEnum.Exception,
                                        av.RoomId,
                                        (int)hostId,
                                        null
                                        );
                    availabilityRepository.AddAvailability(availability);
                }
                 
            }
            Context.SaveChanges();

            return Ok();
        }

        [HttpPut("/availability/edit/{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult UpdateAvailability(int id, [FromBody] EditAvailabilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Availability);
            }

            var availability = availabilityRepository.GetAvailabilityById(id);

            if (availability == null)
            {
                return NotFound();
            }

            availability.StartDate = model.StartDate;
            availability.EndDate = model.EndDate;
            availability.AvailabilityType = model.AvailabilityType;
            availability.RoomId = model.RoomId;
            availability.Occurrence = model.Occurrence;

            Context.SaveChanges();

            return Ok();
        }


    }
}
