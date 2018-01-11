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
using System.Security.Principal;

namespace RSService.Controllers
{
    class SchedulerIdentity : ClaimsIdentity
    {
        private const string UserIdClaim = "http://rsdata.org/claims/user_id";

        private const string EmailClaim = "http://rsdata.org/claims/email";

        public int UserId
        {
            get
            {
                var claimValue = FindFirst(UserIdClaim)?.Value;
                if (claimValue == null)
                    throw new InvalidOperationException();
                return int.Parse(claimValue);
            }
        }
        public string Email
        {
            get
            {
                var claimValue = FindFirst(EmailClaim)?.Value;
                if (claimValue == null)
                    throw new InvalidOperationException();
                return claimValue;
            }
        }

        public SchedulerIdentity(User user)
            : base(CreateClaims(user), "login")
        {

        }

        private SchedulerIdentity(ClaimsIdentity other)
            : base(other)
        {
        }

        public static SchedulerIdentity Current(HttpContext context)
        {
            var currentIdentity = context.User.Identities.First();
            if (currentIdentity.IsAuthenticated)
            {
                return new SchedulerIdentity(currentIdentity);
            }
            return null; 
        }

        private static List<Claim> CreateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(UserIdClaim, user.Id.ToString()),
                new Claim(EmailClaim, user.Email)
            };

            foreach (var userRole in user.UserRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, ((UserRoleEnum)userRole.RoleId).ToString()));
            }

            return claims;
        }
    }

    public class AuthController : BaseController
    {
        private IUserRepository _userRepository;
        private IConfigVarRepository _configVarRepository;
        private ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _userRepository = new UserRepository(Context);
            _configVarRepository = new ConfigVarRepository(Context);
            _logger = logger;
        }

        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Authentication);
            }

            var user = _userRepository.FindUserByCredential(model.LoginName, model.Password);

            if (user == null)
            {
                return ValidationError(GeneralMessages.Authentication);
            }

            var principal = new ClaimsPrincipal(new SchedulerIdentity(user));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          principal,
                                          new AuthenticationProperties
                                          {
                                              IsPersistent = true,
                                              ExpiresUtc = DateTime.UtcNow.AddMinutes(_configVarRepository.GetSessionTimeSpan().Value)
                                          });

            // TODO: return DTO object
            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                departmentId = user.DepartmentId,
                firstName = user.FirstName,
                lastName = user.LastName,
                userRole = new List<int>(user.UserRole.Select(li => li.RoleId)),
                penalty = new List<int>(user.Penalty.Select(li => li.RoomId)),
                isActive = user.IsActive

                //new List<PenaltyDTO>(user.Penalty.Select(li=> new PenaltyDto
                //{
                //    Id = li.Id,
                //    Date = li.Date
                //})
                //)
            });
        }

        [HttpGet("api/auth/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPut("/config/session/edit/{value}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult ChangeSessionTimeSpan(int value)
        {
            if (value == 0)
            {
                return ValidationError(GeneralMessages.ConfigVar);
            }

            var sessionTimeSpan = _configVarRepository.GetSessionTimeSpan();
            sessionTimeSpan.Value = value;

            Context.SaveChanges();

            return Ok();
        }

    }
}
