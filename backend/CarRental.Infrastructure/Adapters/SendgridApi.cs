using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CarRental.Infrastructure.Adapters
{
    public class SendgridApi : IEmailApi
    {
        private readonly IConfigurationSection _sendgridConfig;

        public SendgridApi(IConfigurationSection sendgridConfig)
        {
            _sendgridConfig = sendgridConfig;
        }

        public async Task SendEmail(Domain.Dto.Email email, string emailTo, string userTo)
        {
            var apiKey = Environment.GetEnvironmentVariable(_sendgridConfig["Api-key"]);
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(
                    Environment.GetEnvironmentVariable(_sendgridConfig["FromAddress"]), Environment.GetEnvironmentVariable(_sendgridConfig["FromName"])),
                 new EmailAddress(emailTo, userTo), email.Subject, email.PlainTextContent, email.HtmlContent);
            await client.SendEmailAsync(msg);
        }

        public async Task SendEmails(Email email, List<(string email, string user)> addresses)
        {
            var apiKey = Environment.GetEnvironmentVariable(_sendgridConfig["Api-key"]);
            var client = new SendGridClient(apiKey);
            var emailAddresses = addresses.Select(t => new EmailAddress(t.email, t.user)).ToList();
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(new EmailAddress(
                    Environment.GetEnvironmentVariable(_sendgridConfig["FromAddress"]),
                    Environment.GetEnvironmentVariable(_sendgridConfig["FromName"])),
                emailAddresses, email.Subject, email.PlainTextContent, email.HtmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}