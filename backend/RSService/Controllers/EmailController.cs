using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using RSRepository;
using RSService.DTO;
using RSService.Validation;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RSService.Controllers
{
    public class EmailController : BaseController
    {
        private IUserRepository userRepository;

        public EmailController()
        {
            this.userRepository = new UserRepository(Context);            
        }


        [HttpPost("email/resetpass/{email}")]
        public IActionResult MailPassReset(string email)
        {
            var user = userRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound();
            user.DateExpire = DateTime.UtcNow;
            user.ResetPassCode = System.Guid.NewGuid().ToString();

            Context.SaveChanges();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RoomSchedulerStefanini-noreply", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("User", email));          
            message.Subject = "Password Reset";
            message.Body = new TextPart("html")
            {
                Text = "You have requested a new password for the following account: "+email +"<br>"
                 +
                "If this was a mistake, just ignore this email and nothing will happen. <br> "
                + "If you want to reset you passowrd , visit the following address: <br>"+
                "http://fctestweb1:888/resetpass/" + user.ResetPassCode +"<br>" +
                "For security reasons, this link will expire in 2 hours.To request another password reset, visit http://fctestweb1:888/resetpass <br>"
                + "<br>"+"Best,<br>"+"Your RoomSchedulerTeam"



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
          public IActionResult CheckCodeResetPass(string resetpasscode, [FromForm]ResetPasswordViewModel userView)
           {
            var user = userRepository.GetUserByResetPassCode(resetpasscode);

            if (user == null)
                return NotFound();
            return Ok();
           }

        [HttpPut("/user/resetpass/{ResetPassCode}")]
        public IActionResult ResetPassowrd(string ResetPassCode, [FromBody]ResetPasswordViewModel userView)
        {
           

            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.User);
            }

            var user = userRepository.GetUserByResetPassCode(ResetPassCode);
            

            if (user == null)
            {
                return NotFound(); 
            }

            if (user.ResetPassCode == null)
                return NotFound();

            if ((user.DateExpire.Day != DateTime.UtcNow.Day) || (user.DateExpire.Month != DateTime.UtcNow.Month) || (user.DateExpire.Year != DateTime.UtcNow.Year))
                return NotFound();

            if((user.DateExpire.Day == DateTime.UtcNow.Day) && (user.DateExpire.Month == DateTime.UtcNow.Month) && (user.DateExpire.Year == DateTime.UtcNow.Year))
            {
                if ((DateTime.UtcNow.AddHours(0) > user.DateExpire.AddHours(2)))
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

            Context.SaveChanges();

            var updatedUser = new ResetPassDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                ResetPassCode=user.ResetPassCode


            };

          //  Response.ContentType = "application/JSON";
            return Ok(updatedUser);
        }
    }
}
