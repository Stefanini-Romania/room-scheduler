using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSRepository;
using RSData.Models;
using System.Text;
using RSService.ViewModels;
using static RSData.Models.Role;
using RSService.DTO;
using RSService.Validation;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace RSService.Controllers
{
    public class UserController : BaseController
    {
        private IUserRepository userRepository;
        private IUserRoleRepository userRoleRepository;
        private IRoleRepository roleRepository;
        private IEventRepository eventRepository;

        public UserController()
        {
            this.userRepository = new UserRepository(Context);
            this.userRoleRepository = new UserRoleRepository(Context);
            this.roleRepository = new RoleRepository(Context);
            this.eventRepository = new EventRepository(Context);
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
                    Name = it.Name,
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

        [HttpPost("/user/add")]
       // [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult AddUser([FromBody]UserViewModel newUser)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.User);
            }

            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password));
            newUser.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();
           
            User user = new User()
            {
                Name = newUser.Name,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Password = newUser.Password,
                Email = newUser.Email,
                DepartmentId = newUser.DepartmentId,
                IsActive = true
            };

            userRepository.AddUser(user);
   
            foreach(var roleId in newUser.UserRole)
            {
                userRoleRepository.AddUserRole(new UserRole()
                {
                    UserId = user.Id,
                    RoleId = roleId
                });
            }

            Context.SaveChanges();

            var addedUser = new UserDto()
            {
                Name = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                DepartmentId = user.DepartmentId,
                IsActive = user.IsActive
            };
            return Ok(addedUser);
        }

        [HttpPut("/user/edit/{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult EditUser(int id, [FromBody]EditUserViewModel userView)
        {

            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.User);
            }

            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }         

            //var modifiedUser = Mapper.Map<User>(userView);

            user.Name = userView.Name;
            user.FirstName = userView.FirstName;
            user.LastName = userView.LastName;
            user.Email = userView.Email;
            user.DepartmentId = userView.DepartmentId;
            user.IsActive = userView.IsActive;
            //user.UserRole = userView.UserRole;

            if (userView.Password != null)
            {
                var sha1 = System.Security.Cryptography.SHA1.Create();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(userView.Password));
                user.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            Context.SaveChanges();

            var updatedUser = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                DepartmentId = user.DepartmentId,
                IsActive = user.IsActive

            };

          //  var inactiveusers = userRepository.GetUserByisInactiv();           
            var events = eventRepository.GetEvents();
            if (updatedUser.IsActive == false)
            {
                foreach (var l in events)
                {
                    if (l.AttendeeId == id)
                    {
                        l.EventStatus = 2;
                    }
                }
            }
            Context.SaveChanges();
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