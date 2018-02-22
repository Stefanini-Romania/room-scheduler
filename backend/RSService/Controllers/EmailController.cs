using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using RSRepository;
using RSService.DTO;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RSService.Controllers
{
    public class EmailController : ValidationController
    {
        private readonly IUserRepository _userRepository;
        private readonly RoomPlannerDevContext context;

        public EmailController(RoomPlannerDevContext context)
        {
            this.context = context;
            _userRepository = new UserRepository(context);
        }


        [HttpPost("email/resetpass/{email}")]
        public IActionResult MailPassReset(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            string MatchEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            bool a = Regex.IsMatch(email, MatchEmailPattern);
            if (a == false)
                return NotFound();
            if (user == null)
                return Ok();

            user.DateExpire = DateTime.UtcNow;
            user.ResetPassCode = System.Guid.NewGuid().ToString();

            context.SaveChanges();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RoomSchedulerStefanini-noreply", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("User", email));
            message.Subject = "Password Reset";
            message.Body = new TextPart("html")
            {
                Text ="If you want to reset you passowrd , visit the following address: <br>" +
                "http://fctestweb1:888/resetpass/" + user.ResetPassCode + "<br>" +
                "For security reasons, this link will expire in 2 hours.To request another password reset, visit http://fctestweb1:888/resetpass <br>"




            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("roomchedulerStefanini@gmail.com", "admin123456");

                client.Send(message);

                client.Disconnect(true);
            }

            return Ok();
        }

        [HttpPost("/user/resetpass/{ResetPassCode}")]
        public IActionResult CheckCodeResetPass(string resetpasscode, [FromForm]ResetPasswordDto userView)
        {
            var user = _userRepository.GetUserByResetPassCode(resetpasscode);
        
            if (user == null)
                return NotFound();

            if (user.ResetPassCode == null)
                return NotFound();

            if (user.DateExpire.Date != DateTime.UtcNow.Date)
                return NotFound();

            if (user.DateExpire.Date == DateTime.UtcNow.Date)
            {
                if ((DateTime.UtcNow.AddHours(0) > user.DateExpire.AddHours(1)))
                    return NotFound();
            }

            return Ok();
        }
       

        [HttpPut("/user/resetpass/{ResetPassCode}")]
        public IActionResult ResetPassowrd(string ResetPassCode, [FromBody]ResetPasswordDto userView)
        {
           

            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.User);
            }

            var user = _userRepository.GetUserByResetPassCode(ResetPassCode);
            

            if (user == null)
            {
                return NotFound(); 
            }

            if (user.ResetPassCode == null)
                return NotFound();

            if (user.DateExpire.Date != DateTime.UtcNow.Date)
                return NotFound();

            if(user.DateExpire.Date == DateTime.UtcNow.Date)
            {
                if ((DateTime.UtcNow.AddHours(0) > user.DateExpire.AddMinutes(1)))
                    return NotFound();
            }

            

            user.Password = userView.Password;
            user.ResetPassCode = null;

            if (userView.Password != null)
            {
                var sha1 = System.Security.Cryptography.SHA1.Create();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(userView.Password));
                user.Password = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            context.SaveChanges();

            var updatedUser = new ResetPassDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                ResetPassCode=user.ResetPassCode


            };

            return Ok(updatedUser);
        }
    }
}
