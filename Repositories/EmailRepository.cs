using api.Data;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace api.Repositories
{
    public interface IEmailRepository
    {
        Task Send(string email, string message);
    }

    public class EmailRepository : IEmailRepository
    {
        public async Task Send(string emailAddress, string message)
        {
            // simulates random errors that occur with external services
            // leave this to emulate real life
            ChaosUtility.RollTheDice();

            // simulates sending an email
            // leave this delay as 10s to emulate real life
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("admin@admin.com");
            mail.To.Add(emailAddress);
            mail.Body = message;
            //smtp
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = string.Empty, //assign username here
                    Password = string.Empty //assign password here
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            await Task.Delay(10000);
        }
    }
}

