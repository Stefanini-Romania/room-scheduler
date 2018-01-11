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


        [HttpPost("email/resetpass/{sendmail}")]
        public IActionResult MailPassReset(string sendmail)
        {
            var user = userRepository.GetUserByEmail(sendmail);
            user.DateExpire = DateTime.UtcNow;

            Context.SaveChanges();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RoomSchedulerStefanini-noreply", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("User", sendmail));
            message.Subject = "Password Reset";
            message.Body = new TextPart("html")
            {
                Text = "Someone has requested a password reset for the following account: "+sendmail +"<br>"
                 +
                "If this was a mistake, just ignore this email and nothing will happen. <br> "
                + "If you want to reset you passowrd , visit the following address: <br>"+
                "http://localhost:4200/calendarr"



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

        [HttpPut("/user/resetPass/{email}")]
        public IActionResult ResetPassowrd(string email, [FromBody]ResetPasswordViewModel userView)
        {


            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.User);
            }

            var user = userRepository.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound(); 
            }

            if ((user.DateExpire.Day != DateTime.UtcNow.Day) || (user.DateExpire.Month != DateTime.UtcNow.Month) || (user.DateExpire.Year != DateTime.UtcNow.Year))
                return NotFound();

            if((user.DateExpire.Day == DateTime.UtcNow.Day) && (user.DateExpire.Month == DateTime.UtcNow.Month) && (user.DateExpire.Year == DateTime.UtcNow.Year))
            {
                if ((DateTime.UtcNow.AddHours(0) > user.DateExpire.AddHours(2)))
                    return NotFound();
            }



            //var modifiedUser = Mapper.Map<User>(userView);

            user.Password = userView.Password;

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
                Password = user.Password


            };


            return Ok(updatedUser);
        }
    }
}
