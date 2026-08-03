using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Common.SharedEndpoints
{
    public class AppInfo : ICarterModule
    {
        public record AppInfoResponse(
            string ApiName,
            string Environment
            );

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("app-info", () =>
            {
                var appInfo = new AppInfoResponse(
                    ApiName: Assembly.GetEntryAssembly()?.GetName().Name ?? SharedEndpointConstants.DefaultApiName,
                    Environment: Environment.GetEnvironmentVariable(SharedEndpointConstants.EnvironmentVariableName) ?? SharedEndpointConstants.DefaultEnvironment
                );
                return Results.Ok(appInfo);
            })
            .WithName("App Info")
            .WithTags("SharedEndpoints")
            .Produces<AppInfoResponse>(StatusCodes.Status200OK)
            .WithOpenApi();
        }
    }
}
