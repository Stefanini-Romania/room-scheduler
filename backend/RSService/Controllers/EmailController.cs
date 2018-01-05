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
        public IActionResult PassReset(string sendmail)
        {           

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RoomSchedulerStefanini-noreply", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("User", sendmail));
            message.Subject = "Passowrd Reset";
            message.Body = new TextPart("html")
            {
                Text = "Someone has requested a password reset for the following account: "+sendmail +"<br>"
                 +
                "If this was a mistake, just ignore this email and nothing will happen. <br> "
                + "If you want to reset you passowrd , visit the following address: <br>"



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
