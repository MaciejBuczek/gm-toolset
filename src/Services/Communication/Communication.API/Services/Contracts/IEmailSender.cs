namespace Communication.API.Services.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string mailTo, string subject, string htmlContent, CancellationToken cancellationToken = default);
    }
}
