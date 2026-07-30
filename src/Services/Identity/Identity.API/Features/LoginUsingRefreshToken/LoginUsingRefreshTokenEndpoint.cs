namespace Identity.API.Features.LoginUsingRefreshToken
{
    public class LoginUsingRefreshTokenEndpoint : ICarterModule
    {
        public record LoginUsingRefreshTokenCommandResponse(string Token, string RefreshToken);
        public record LoginUsingRefreshTokenCommandRequest(string RefreshToken);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/login/refresh",
                async (LoginUsingRefreshTokenCommand request,
                IRequestHandler<LoginUsingRefreshTokenCommand, LoginUsingRefreshTokenCommandResult> handler,
                CancellationToken cancellationToken) =>
            {
                var command = request.Adapt<LoginUsingRefreshTokenCommand>();
                var result = await handler.Handle(command, cancellationToken);
                var response = result.Adapt<LoginUsingRefreshTokenCommandResponse>();

                return Results.Ok(response);
            }).WithName("Refresh")
            .WithTags("Identity")
            .Produces<LoginCommandResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithOpenApi();
        }
    }
}
