using AutoMapper;
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
                    finalResults.Add(new AvailabilityDto(ex.Id, ex.StartDate, ex.EndDate, ex.AvailabilityType, ex.Room.Name));
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
                    if (av.StartDate.Date >= startDate && av.StartDate.Date <= startDate.AddDays(4).Date)
                    {
                        results.Add(new AvailabilityDto(av.Id, av.StartDate, av.EndDate, av.AvailabilityType, av.Room.Name));
                    }
                }
                else
                {
                    DateTime date = av.StartDate.AddDays(-(int)av.StartDate.DayOfWeek + 1);
                    while (date.Date <= startDate.Date)
                    {
                        if (date.Date == startDate.Date)
                        {
                            results.Add(new AvailabilityDto(
                                            av.Id,
                                            new DateTime(date.Year, date.Month, date.Day, av.StartDate.Hour, av.StartDate.Minute, av.StartDate.Second).AddDays((int)av.StartDate.DayOfWeek - 1), 
                                            new DateTime(date.Year, date.Month, date.Day, av.EndDate.Hour, av.EndDate.Minute, av.EndDate.Second).AddDays((int)av.StartDate.DayOfWeek - 1),
                                            av.AvailabilityType, 
                                            av.Room.Name));
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
            var results = userRepository.GetActiveHosts();
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
        public IActionResult AddAvailability([FromBody] AddAvailabilityDto newAvailability, int? hostId)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Availability);
            }

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
                                        newAvailability.StartDate.AddDays(day - 1),
                                        newAvailability.EndDate.AddDays(day - 1),
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
                        newAvailability.StartDate.AddDays(day - 1),
                        newAvailability.EndDate.AddDays(day - 1),
                        (int)AvailabilityEnum.Available,
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
                //Find the availabilities for that day that intersect exception and add exception for each room
                var availabilities = availabilityRepository.GetAvailabilitiesByHostAndDate(currentUser.Id, avException.StartDate, avException.EndDate);

                if (!availabilities.Any())
                {
                    return ValidationError(AvailabilityMessages.InvalidTime);
                }

                foreach (var av in availabilities)
                {
                    DateTime newStart;
                    DateTime newEnd;

                    if (avException.StartDate.TimeOfDay.Ticks > av.StartDate.TimeOfDay.Ticks)
                    {
                        newStart = avException.StartDate;
                    }
                    else
                    {
                        newStart = new DateTime(avException.StartDate.Year, avException.StartDate.Month, avException.StartDate.Day, av.StartDate.Hour, av.StartDate.Minute, av.StartDate.Second);
                    }

                    if (avException.EndDate.TimeOfDay.Ticks < av.EndDate.TimeOfDay.Ticks)
                    {
                        newEnd = avException.EndDate;
                    }
                    else
                    {
                        newEnd = new DateTime(avException.EndDate.Year, avException.EndDate.Month, avException.EndDate.Day, av.EndDate.Hour, av.EndDate.Minute, av.EndDate.Second);
                    }

                    Availability availability = new Availability(
                                newStart,
                                newEnd,
                                (int)AvailabilityEnum.Exception,
                                av.RoomId,
                                currentUser.Id,
                                0
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
                //Find the availabilities for that day that intersect exception and add exception for each room
                var availabilities = availabilityRepository.GetAvailabilitiesByHostAndDate((int)hostId, avException.StartDate, avException.EndDate);

                if (!availabilities.Any())
                {
                    return ValidationError(AvailabilityMessages.InvalidTime);
                }

                foreach (var av in availabilities)
                {
                    DateTime newStart;
                    DateTime newEnd;

                    if (avException.StartDate.TimeOfDay.Ticks > av.StartDate.TimeOfDay.Ticks)
                    {
                        newStart = avException.StartDate;
                    }
                    else
                    {
                        newStart = new DateTime(avException.StartDate.Year, avException.StartDate.Month, avException.StartDate.Day, av.StartDate.Hour, av.StartDate.Minute, av.StartDate.Second);
                    }

                    if (avException.EndDate.TimeOfDay.Ticks < av.EndDate.TimeOfDay.Ticks)
                    {
                        newEnd = avException.EndDate;
                    }
                    else
                    {
                        newEnd = new DateTime(avException.EndDate.Year, avException.EndDate.Month, avException.EndDate.Day, av.EndDate.Hour, av.EndDate.Minute, av.EndDate.Second);
                    }

                    Availability availability = new Availability(
                                newStart,
                                newEnd,
                                (int)AvailabilityEnum.Exception,
                                av.RoomId,
                                (int)hostId,
                                0
                                );
                    availabilityRepository.AddAvailability(availability);
                }
            }
            Context.SaveChanges();

            return Ok();
        }

        [HttpPut("/availability/edit/{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult UpdateAvailability(int id, [FromBody] EditAvailabilityDto model)
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

            if (availability.AvailabilityType == (int)AvailabilityEnum.Exception)
            {
                availability.Status = model.Status;
            }
            else
            {
                availability.StartDate = model.StartDate;
                availability.EndDate = model.EndDate;
                availability.RoomId = model.RoomId;
                availability.Occurrence = model.Occurrence;
                availability.Status = model.Status;
            }

            

            Context.SaveChanges();

            return Ok();
        }


    }
}
