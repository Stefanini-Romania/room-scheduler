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
using Microsoft.AspNetCore.Authentication.Cookies;
using RSService.DTO;

namespace RSService.Controllers
{

    public class AuthController : BaseController
    {
        private IUserRepository _userRepository;
        private ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _userRepository = new UserRepository(Context);
            _logger = logger;
        }

        [HttpPost("api/auth/login")]
        //[AuthenticationValidator]  --attribute
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Authentication);
            }

            var user = _userRepository.FindUserByCredential(model.Name, model.Password);

            if (user == null )
            {
                return ValidationError(GeneralMessages.Authentication);
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Name)
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // TODO: return DTO object
            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                name = user.Name,
                departmentId = user.DepartmentId,
                userRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                penalty = new List<PenaltyDto>(user.Penalty.Select(li => new PenaltyDto
                                                                         { Id = li.Id,
                                                                           Date = li.Date })
                                              )
            });
        }

        [HttpGet("api/auth/logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
