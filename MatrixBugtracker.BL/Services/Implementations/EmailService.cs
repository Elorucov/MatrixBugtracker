using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailConfig> options, ILogger<EmailService> logger)
        {
            _config = options.Value;
            _logger = logger;
        }

        public async Task SendMailAsync(string destination, string subject, string text)
        {
            try
            {
                using SmtpClient client = new SmtpClient(_config.Host);
                client.Port = _config.Port;
                client.Credentials = new NetworkCredential(_config.Login, _config.Password);
                client.EnableSsl = true;

                bool result = MailAddress.TryCreate(_config.Login, out MailAddress from);
                if (!result)
                {
                    _logger.LogError("Cannot create MailAddress instance! Destination: {0}; subject: \"{1}\"; message: \"{2}\"", destination, subject, text);
                    return;
                }

                bool testMode = !string.IsNullOrEmpty(_config.DestinationOverride);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = from;
                mailMessage.To.Add(testMode ? _config.DestinationOverride : destination);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = subject;
                mailMessage.Body = text;

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email successfully sent to {0}. Subject: {1}", destination, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot send email! Destination: {0}; subject: \"{1}\"; message: \"{2}\"", destination, subject, text);
            }
        }
    }
}
