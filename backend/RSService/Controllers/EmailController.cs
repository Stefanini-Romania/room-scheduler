using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RSService.Controllers
{
    public class EmailController : Controller
    {
        [HttpPost("api/email/send")]
        public IActionResult Send()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Test Project", "roomchedulerStefanini@gmail.com"));
            message.To.Add(new MailboxAddress("Scrum Meeting", "lgbogdan12@gmail.com"));
            message.Subject = "Test email";
            message.Body = new TextPart("plain")
            {
                Text="Hello Room Scheduler"
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
