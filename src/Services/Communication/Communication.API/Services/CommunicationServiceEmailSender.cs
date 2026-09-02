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

            try
            {
                await EmailClient.SendAsync(WaitUntil.Completed, message, cancellationToken);
            }
            catch (RequestFailedException ex) when (IsRetryableException(ex))
            {
                throw new RetryableException(ex.Message);
            }
        }

        private static bool IsRetryableException(Exception ex)
        {
            return ex is RequestFailedException requestFailedException &&
                   (
                   requestFailedException.Status == 408 || // Request Timeout
                    requestFailedException.Status == 429 || // Too Many Requests
                    requestFailedException.Status >= 500);   // Azure Service errors (5xx)
        }
    }
}
