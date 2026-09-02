namespace Communication.API.Services.Contracts
{
    public interface IEmailTemplateRenderer
    {
        Task<string> RenderTemplateAsync<TModel>(string templateName, TModel model, CancellationToken cancellationToken = default); 
    }
}
