using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSRepository;
using RSData.Models;
using System.Text;

namespace RSService.Controllers
{
    public class AdminController : BaseController
    {
        private IDbOperation dbOperation;
        private IUserRepository userRepository;

        public AdminController(IDbOperation _dbOperation, IUserRepository _userRepository)
        {
            dbOperation = _dbOperation;
            userRepository = _userRepository;
        }

        [HttpPost("/admin/adduser")]
        public IActionResult AddUser([FromBody]User model)
        {
            if (model.Name == null || model.Password == null)
            {
                return BadRequest();
            }
            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            model.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();

            userRepository.AddUser(model);
            dbOperation.Commit();
   
            return Ok(model);
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}