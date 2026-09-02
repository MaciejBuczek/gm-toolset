namespace Communication.API.Services
{
    public class CommunicationServiceEmailSender(EmailClient EmailClient, IOptions<AzureCommunicationService> ConfigurationOptions) : IEmailSender
    {
        public async Task SendEmailAsync(string mailTo, string subject, string htmlContent, CancellationToken cancellationToken = default)
        {
            var content = new EmailContent(subject)
            {
                Html = htmlContent
            };
            var recipients = new EmailRecipients([new EmailAddress(mailTo)]);
            var message = new EmailMessage(ConfigurationOptions.Value.SenderAddress, recipients, content);
            await EmailClient.SendAsync(Azure.WaitUntil.Started, message, cancellationToken);
        }
    }
}
