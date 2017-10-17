using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSData.Models;
using RSRepository;
using RSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace RSService.Controllers
{
    
    public class AuthController : Controller
    {
        private IUserRepository _userRepository;
        private ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        private bool LoginUser(string username, string password)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(c => c.Name == username);

            if (user == null)
            {
                return false;
            }

            if (!user.Password.Equals(password))
            {
                return false;
            }

            return true;
        }

        [HttpPost("api/auth/login")]
        //[ValidateModel]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
            if (LoginUser(model.UserName, model.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                //Just redirect to our index after logging in. 
                return Ok();
            }
            return BadRequest("Failed to login");
        }





    }
}
