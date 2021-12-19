using System;
using System.Threading.Tasks;
using CarRental.Domain.Ports.Out;
using CarRental.Domain.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CarRental.Infrastructure.Email
{
    public class SendgridApi : IEmailApi
    {
        private readonly IConfigurationSection _sendgridConfig;

        public SendgridApi(IConfigurationSection sendgridConfig)
        {
            _sendgridConfig = sendgridConfig;
        }

        public async Task SendEmail(Domain.Dto.Email email)
        {
            var apiKey = Environment.GetEnvironmentVariable(_sendgridConfig["Api-key"]);
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(
                    Environment.GetEnvironmentVariable(_sendgridConfig["FromAddress"]), Environment.GetEnvironmentVariable(_sendgridConfig["FromName"])),
                 new EmailAddress(email.ToMail, email.ToName), email.Subject, email.PlainTextContent, email.HtmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}