namespace Identity.API.Features.Login
{
    public class LoginEndpoint : ICarterModule
    {
        public record LoginCommandResponse(string Token, string RefreshToken);
        public record LoginCommandRequest(string Username, string Email, string Password);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/login",
                async (LoginCommandRequest request,
                IRequestHandler<LoginCommand, LoginCommandResult> handler,
                CancellationToken cancellationToken) =>
            {
                var query = request.Adapt<LoginCommand>();
                var result = await handler.Handle(query, cancellationToken);
                var response = result.Adapt<LoginCommandResponse>();

                return Results.Ok(response);
            })
            .WithName("Login")
            .WithTags("Identity")
            .Produces<LoginCommandResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithOpenApi();
        }
    }
}
