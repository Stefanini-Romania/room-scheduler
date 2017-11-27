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

namespace RSService.Controllers
{
    public class AdminController : BaseController
    {
        private IUserRepository userRepository;
        private IUserRoleRepository userRoleRepository;
        private IRoleRepository roleRepository;

        public AdminController(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.roleRepository = roleRepository;
        }

        [HttpPost("/admin/adduser")]
        public IActionResult AddUser([FromBody]UserModel model)
        {
            if (model.Name == null || model.Password == null)
            {
                return BadRequest();
            }
            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            model.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();

           
            User newUser = new RSData.Models.User()
            {
                Name = model.Name,
                Password = model.Password,
                Email = model.Email,
                DepartmentId = model.DepartmentId                
            };
         
            userRepository.AddUser(newUser);
            var my = true;
            userRoleRepository.AddUserRole(new UserRole()
            {
                User = newUser,
                UserId = newUser.Id,
                RoleId = model.RoleId,
                Role = roleRepository.GetRoleById(model.RoleId)
            });
           
            Context.SaveChanges();
            return Ok(model);
        }

        [HttpPost("/admin/edituser/{id}")]
        public IActionResult EditUser(int id, [FromBody]User model)
        {
            var user = userRepository.GetUsers().FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return BadRequest("No such user");
            }
            if (model.Name != null)
                user.Name = model.Name;
            if (model.Password != null)
            {
                var sha1 = System.Security.Cryptography.SHA1.Create();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                user.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            if (model.Email != null)
                user.Email = model.Email;
            if (model.DepartmentId != null)
                user.DepartmentId = model.DepartmentId;
            userRepository.UpdateUser(user);
            return Ok(user);
        }

        [HttpPost("/admin/deleteuser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return BadRequest("No such user");
            }
            userRepository.DeleteUser(user);
            return Ok();
        }
    }
}