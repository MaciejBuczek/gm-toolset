namespace Communication.API.Services
{
    public class RazorRenderer(IServiceProvider ServiceProvider, IRazorViewEngine RazorViewEngine, ITempDataProvider TempDataProvider) : IEmailTemplateRenderer
    {
        public async Task<string> RenderTemplateAsync<TModel>(string templateName, TModel model, CancellationToken cancellationToken = default)
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = ServiceProvider
            };

            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor());

            var viewName = $"~/Emails/Templates/{templateName}";

            var viewResult = RazorViewEngine.GetView(
                executingFilePath: null,
                viewPath: viewName,
                isMainPage: true);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException(
                    $"Email template '{templateName}' was not found.");
            }

            await using var writer = new StringWriter();

            var viewDictionary = new ViewDataDictionary<TModel>(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(
                    httpContext,
                    TempDataProvider),
                writer,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);

            return writer.ToString();
        }
    }
}
