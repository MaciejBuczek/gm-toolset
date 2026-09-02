namespace Communication.API.Services
{
    public class MockedEmailSender(ILogger<MockedEmailSender> Logger) : IEmailSender
    {
        public Task SendEmailAsync(string mailTo, string subject, string htmlContent, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Mocked email sent to {MailTo} with subject '{Subject}' and content: {HtmlContent}", mailTo, subject, htmlContent);
            return Task.CompletedTask;
        }
    }
}
