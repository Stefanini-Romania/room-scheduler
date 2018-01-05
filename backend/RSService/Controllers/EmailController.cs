using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using RSRepository;
using RSService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult PassReset(string sendmail /*[FromBody]EditUserViewModel userView*/)
        {
           // var user = userRepository.GetUserByEmail(email);
           // if (user == null)
           // {
           //     return NotFound();
           // }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RoomSchedulerStefanini", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("Scrum Meeting", sendmail));
            message.Subject = "Passowrd Reset";
            message.Body = new TextPart("plain")
            {
                Text="Sadly we can't do anything for you right now. "
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
    }
}
