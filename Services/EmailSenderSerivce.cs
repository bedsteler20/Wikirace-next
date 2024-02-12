namespace Wikirace.Services;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


public class EmailSenderOptions {
    public string? SendGridKey { get; set; }
}

public class EmailSenderService(IOptions<EmailSenderOptions> optionsAccessor,
                   ILogger<EmailSenderService> logger) : IEmailSender {
    private readonly ILogger _logger = logger;

    public EmailSenderOptions Options { get; } = optionsAccessor.Value;

    public async Task SendEmailAsync(string toEmail, string subject, string message) {
        if (string.IsNullOrEmpty(Options.SendGridKey)) {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail) {
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