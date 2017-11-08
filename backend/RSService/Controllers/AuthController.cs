using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSData.Models;
using RSRepository;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using RSService.Validation;

namespace RSService.Controllers
{

    public class AuthController : BaseController
    {
        private IUserRepository _userRepository;
        private ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost("api/auth/login")]
        //[AuthenticationValidator]  --attribute
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
            if (!ModelState.IsValid)
            {
                return (new ValidationFailedResult(GeneralMessages.Authentication, ModelState));
            }

            if (_userRepository.FindUserByCredential(model.Name, model.Password) == null )
            {
                return (new ValidationFailedResult(GeneralMessages.Authentication, ModelState));
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Name)
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            var user = _userRepository.GetUsers().FirstOrDefault(c => c.Name == model.Name);
            //Just redirect to our index after logging in. 
            return Ok(user);
            
            
        }





    }
}
