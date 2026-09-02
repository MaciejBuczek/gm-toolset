namespace Communication.API.Events.UserCreatedEvent
{
    public class UserCreatedIntegrationEventConsumer(IEmailSender EmailSender, IEmailTemplateRenderer EmailTemplateRenderer) : IConsumeAsync<UserCreatedIntegrationEvent>
    {
        public async Task ConsumeAsync(UserCreatedIntegrationEvent message, CancellationToken cancellationToken = default)
        {
            var model = new WelcomeEmailModel(Username: message.Username);
            var content = await EmailTemplateRenderer.RenderTemplateAsync("WelcomeEmail.cshtml", model, cancellationToken);

            await EmailSender.SendEmailAsync(message.Email, "Welcome to GM-Toolest!", content, cancellationToken);
        }
    }
}
