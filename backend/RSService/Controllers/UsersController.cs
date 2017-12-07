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

namespace RSService.Controllers
{
    public class UsersController : BaseController
    {
        private IUserRepository userRepository;
        private IUserRoleRepository userRoleRepository;
        private IRoleRepository roleRepository;

        public UsersController(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.roleRepository = roleRepository;
        }

        [HttpGet("/users/list")]
        public IActionResult GetUsers(int limit, int page)
        {
            if (limit == 0) limit = 8;
            if (page == 0) page = 1;

            var results = userRepository.GetUsers(limit, page);
            if (results == null)
                return NotFound();

            var userCount = userRepository.GetUsers().Count();
            
            List<UserDTO> final_result = new List<UserDTO>();

            foreach (var it in results)
            {
                final_result.Add(new UserDTO()
                {
                    Id = it.Id,
                    Name = it.Name,
                    FirstName = it.FirstName,
                    LastName = it.LastName,
                    Email = it.Email,
                    UserRole = new List<int>(it.UserRole.Select(li => li.RoleId)),
                    DepartmentId = it.DepartmentId,
                    UserTotalNumber = userCount
                });
            }

            return Ok(final_result);
        }

        [HttpPost("/users/add")]
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
                DepartmentId = newUser.DepartmentId                
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

            var addedUser = new UserDTO()
            {
                Name = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                DepartmentId = user.DepartmentId
            };
            return Ok(addedUser);
        }

        [HttpPost("/users/edit/{id}")]
        public IActionResult EditUser(int id, [FromBody]UserViewModel userView)
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

            user.Name = userView.Name;
            user.Email = userView.Email;
            user.DepartmentId = userView.DepartmentId;

            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(userView.Password));
            user.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();

            Context.SaveChanges();

            var updatedUser = new UserDTO()
            {
                Name = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                DepartmentId = user.DepartmentId
            };
            return Ok(updatedUser);
        }

        [HttpDelete("/users/delete/{id}")]
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