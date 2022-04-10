using Bookstore.WebApi.Data.Entities.Configuration;
using Bookstore.WebApi.Data.Interface;
using Bookstore.WebApi.ViewModel;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Bookstore.WebApi.Data.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration emailConfig;
        public EmailService(EmailConfiguration emailConfig)
        {
            this.emailConfig = emailConfig;
        }
        public async Task SendEmailAsync(MessageEmail message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }
        private MailMessage CreateEmailMessage(MessageEmail message)
        {
            MailAddress from = new MailAddress(emailConfig.From);
            MailAddress to = new MailAddress(message.To);
            var emailMessage = new MailMessage(from, to);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = message.Content;
            return emailMessage;
        }
        private async Task SendAsync(MailMessage mailMessage)
        {
            using (var client = new SmtpClient(emailConfig.SmtpServer, emailConfig.Port))
            {
                try
                    {
                        await client.SendMailAsync(mailMessage);
                    }
                    catch
                    {
                        //log an error message or throw an exception or both.
                        throw;
                    }
                    finally
                    {
                        client.Dispose();
                    }
            };
        } 
    }
    
}