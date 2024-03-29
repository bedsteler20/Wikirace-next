namespace Wikirace.Services;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


public class EmailSenderOptions {
    public string? SendGridKey { get; set; }
}

/// <summary>
/// Represents a service for sending emails.
/// </summary>
public class EmailSenderService : IEmailSender {
    private readonly ILogger _logger;

    public EmailSenderService(IOptions<EmailSenderOptions> optionsAccessor,
                       ILogger<EmailSenderService> logger) {
        _logger = logger;
        Options = optionsAccessor.Value;
    }

    public EmailSenderOptions Options { get; }

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The content of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEmailAsync(string toEmail, string subject, string message) {
        if (string.IsNullOrEmpty(Options.SendGridKey)) {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    private async Task Execute(string apiKey, string subject, string message, string toEmail) {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage() {
            From = new EmailAddress("noreply@wikirace.app"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        if (response.IsSuccessStatusCode) {
            _logger.LogInformation("Email to {toEmail} queued successfully!", toEmail);
        } else {
            _logger.LogInformation("Failure Email to {toEmail}", toEmail);
        }
    }
}