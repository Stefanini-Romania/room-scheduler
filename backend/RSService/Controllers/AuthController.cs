using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSData.Models;
using RSRepository;
using RSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Controllers
{
    public class AuthController : Controller
    {
        private RoomPlannerDevContext _context;
        private SignInManager<User> _signInManager;
        private ILogger<AuthController> _logger;

        public AuthController(RoomPlannerDevContext context, SignInManager<User> signInManager, ILogger<AuthController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("api/auth/login")]
        //[ValidateModel]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in: {ex}");
            }

            return BadRequest("Failed to login");
        }





    }
}
