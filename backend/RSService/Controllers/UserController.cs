using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSRepository;
using RSData.Models;
using System.Text;
using static RSData.Models.Role;
using RSService.DTO;
using RSService.Validation;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MailKit.Net.Smtp;


namespace RSService.Controllers
{
    public class UserController : ValidationController
    {
        
        private readonly IUserRepository userRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IEventRepository eventRepository;
        private readonly ISettingsRepository settingsRepository;
        private readonly IRoomRepository roomRepository;
        private readonly RoomPlannerDevContext context;

        public UserController(RoomPlannerDevContext context)
        {
            this.context = context;
            this.userRepository = new UserRepository(context);
            this.userRoleRepository = new UserRoleRepository(context);
            this.roleRepository = new RoleRepository(context);
            this.eventRepository = new EventRepository(context);
            this.settingsRepository = new SettingsRepository(context);
            this.roomRepository = new RoomRepository(context);   
        }

        [HttpGet("/user/list")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult GetUsers()
        {
            var results = userRepository.GetUsers();
            if (results == null)
                return NotFound();
            
            List<UserDto> final_result = new List<UserDto>();

            foreach (var it in results)
            {
               final_result.Add(new UserDto()
                {
                    Id = it.Id,
                    FirstName = it.FirstName,
                    LastName = it.LastName,
                    Email = it.Email,
                    UserRole = new List<int>(it.UserRole.Select(li => li.RoleId)),
                    DepartmentId = it.DepartmentId,
                    IsActive = it.IsActive
                });
            }

            return Ok(final_result);
        }

        [HttpPost("users/reminder")]
        public IActionResult EventReminder()
        {

            //this will olways will have just one value, so it doesn't matter it's for in for;
            var emailremindervalue = settingsRepository.GetValueOfEmailReminderSettings();
            foreach (Settings set in emailremindervalue)
            {
                var events = eventRepository.GetEventsByDateTimeNow(Int32.Parse(set.Value));
                foreach (Event evnt in events)
                {
                    evnt.EventStatus = (int)EventStatusEnum.waitingRemindet;
                    context.SaveChanges();
                    var usr = userRepository.GetUserById(evnt.AttendeeId);
                    var room = roomRepository.GetRoomById(evnt.RoomId);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("RoomSchedulerStefanini", "roomchedulerStefanini@gmail.com"));
                    message.To.Add(new MailboxAddress("User", usr.Email));
                    message.Subject = "Remainder";
                    message.Body = new TextPart("html")
                    {
                        Text = " You have a massage programmed for today! <br>"
                        + " DateStart: " + evnt.StartDate.TimeOfDay + "<br>"
                        + " Room Name: " + room.Name + "<br>"
                        + " Room Location: " + room.Location + "<br>"



                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("roomchedulerStefanini@gmail.com", "admin123456");

                        client.Send(message);

                        client.Disconnect(true);
                    }

                }
            }
            return Ok();
        }

        [HttpPost("/user/add")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult AddUser([FromBody]AddUserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.User);
            }

            var schedulerIdentity = SchedulerIdentity.Current(HttpContext);
            var currentUserId = schedulerIdentity.UserId;
            var currentUser = userRepository.GetUserById(currentUserId);

            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password));
            newUser.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();
            User user = new User(newUser.Email, newUser.Password, newUser.DepartmentId, newUser.FirstName, newUser.LastName, true, DateTime.UtcNow);
            
            userRepository.AddUser(user);
   
            foreach(var roleId in newUser.UserRole)
            {
                userRoleRepository.AddUserRole(new UserRole()
                {
                    UserId = user.Id,
                    RoleId = roleId
                });
            }

            context.SaveChanges();

            var addedUser = new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                DepartmentId = user.DepartmentId,
                IsActive = user.IsActive

               
            };
            //sss


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RoomSchedulerStefanini", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("User", user.Email));
            message.Subject = "Welcome!";
            message.Body = new TextPart("html")
            {
                Text = " You received a new account.<br>"
                +"Username: "+ user.Email + "<br>" 

            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("roomchedulerStefanini@gmail.com", "admin123456");

                client.Send(message);

                client.Disconnect(true);
            }
            return Ok(addedUser);
           
        }
        [HttpPost("/user/register")]       
        public IActionResult RegisterUser([FromBody]RegisterUserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.User);
            }

            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password));
            newUser.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();


            User user = new User(newUser.Email, newUser.Password, newUser.DepartmentId, newUser.FirstName, newUser.LastName, true, DateTime.UtcNow);

            userRepository.AddUser(user);

            userRoleRepository.AddUserRole(new UserRole()
            {
                UserId = user.Id,
                RoleId = (int)UserRoleEnum.attendee
            });

            context.SaveChanges();


            var addedUser = new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                DepartmentId = user.DepartmentId,
                IsActive = user.IsActive

            };

            //if (user.Email != "")
            //{
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("RoomSchedulerStefanini", "roomchedulerStefanini@gmail.com"));
                message.To.Add(new MailboxAddress("User", user.Email));
                message.Subject = "Welcome!";
                message.Body = new TextPart("html")
                {
                    Text = " Welcome to RoomScheduler!"
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("roomchedulerStefanini@gmail.com", "admin123456");

                    client.Send(message);

                    client.Disconnect(true);
                }
            //}
            return Ok(addedUser);
        }

        [HttpPut("/user/edit/{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult EditUser(int id, [FromBody]EditUserDto userView)
        {

            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.UserEdit);
            }

            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }         

            //var modifiedUser = Mapper.Map<User>(userView);

            user.FirstName = userView.FirstName;
            user.LastName = userView.LastName;
            user.Email = userView.Email;
            user.DepartmentId = userView.DepartmentId;
            user.IsActive = userView.IsActive;
    

            // If user is inactive change his events status to cancelled
            if (user.IsActive == false)
            {
                var events = eventRepository.GetEventsByUser(id);

                foreach (var e in events)
                {
                    if (e.EventStatus == (int)EventStatusEnum.waiting)
                    {
                        e.EventStatus = (int)EventStatusEnum.cancelled;
                    }
                }
            }

            if(userView.UserRole != null)
            {
                for (int i = 1; i <= 3; i++)
                {

                    // If user has role i
                    if (userRoleRepository.GetUserRoleByUserAndRole(id, i) != null)
                    {
                        int hasRole = 0;

                        // Verify if user still have this role in View
                        foreach (var roleId in userView.UserRole)
                        {
                            if (roleId == i) hasRole = 1;
                        }

                        //If not, remove userrole
                        if (hasRole == 0)
                        {
                            userRoleRepository.RemoveUserRole(id, i);
                        }
                    }
                    else  //If user doesn't have role i
                    {
                        // Verify if user have this role in View. If so, add new userrole
                        foreach (var roleId in userView.UserRole)
                        {
                            if (roleId == i)
                            {
                                userRoleRepository.AddUserRole(new UserRole()
                                {
                                    UserId = user.Id,
                                    RoleId = roleId
                                });
                            }
                        }
                    }
                }

            }

            if (userView.Password != null)
            {
                var sha1 = System.Security.Cryptography.SHA1.Create();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(userView.Password));
                user.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            context.SaveChanges();

            var updatedUser = new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                DepartmentId = user.DepartmentId,
                IsActive = user.IsActive,
   
                

            };
          
            return Ok(updatedUser);
        }

        //[HttpDelete("/user/delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            userRepository.DeleteUser(user);
            return Ok();
        }
    }
}