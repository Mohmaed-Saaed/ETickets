using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace ETickets.Servies
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com") // the protocal of emails
            {
                EnableSsl = true, // ensures that the request is coming from https
                Port = 587, // this is the port
                Credentials = new NetworkCredential("muhameds913@gmail.com", "wocs xdhw knew djkk"), // the sender
            };

            return smtpClient.SendMailAsync(
                new MailMessage(from: "muhameds913@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                ) { IsBodyHtml = true}
                );

        }
    }
}
