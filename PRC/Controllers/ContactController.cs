using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace PRC.Controllers
{
    public class ContactController : Controller
    {
        [BindProperty]
        public ContactFormModel Contact { get; set; }

        public class ContactFormModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
            public string Target { get; set; }
        }

        public IActionResult Index([FromBody] ContactFormModel contact)
        {
            // create and send the mail here
            var mailbody = $@"Hello PRC,<br /><br />

            This is a new contact request from the website:<br /><br />

            Name: {contact.Name} <br />
            Email: {contact.Email} <br />
            Message: {contact.Message} <br /><br />

            <a href='mailto:{contact.Email}?subject=Reply From Pierpont Racquet Club Membership'>Click Here To Reply</a>";

            SendMail(mailbody, contact.Email, contact.Target);

            return Json("ok");
        }

        private void SendMail(string mailbody, string email, string target)
        {
            var contact = target + "@pierpontrc.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email));
            message.To.Add(new MailboxAddress("Pierpont Racquet Club", contact));
            message.Subject = "New request for contact from Pierpont Racquet Club.";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailbody;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("pierpontmailer@gmail.com", "ggdilioqfnupadle");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}