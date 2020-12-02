using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BasicApi.Items.Dtos;
using BasicApi.Items.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public class EmailService : ServiceBase, IEmailService
    {
        private IOptions<EmailOptions> Options { get; }
        private ILogger<EmailService> Logger { get; }

        public EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
        {
            Options = options;
            Logger = logger;
        }
        public async Task SendEmailAsync(SendEmailDto sendEmail)
        {
            SendGridClient client = new SendGridClient(Options.Value.ServiceKey);

            SendGridMessage message = new SendGridMessage
            {
                From = new EmailAddress(Options.Value.FromEmail, Options.Value.FromName),
                Subject = sendEmail.Subject,
                PlainTextContent = sendEmail.Message,
                HtmlContent = sendEmail.Message
            };
            message.AddTo(sendEmail.To);

            Response response = await client.SendEmailAsync(message);

            Logger.LogInformation($"Send email response with: {response.StatusCode.ToString()}");
        }
    }
}
