namespace Identity.API.Features.Login
{
    public class LoginEndpoint : ICarterModule
    {
        public record LoginResponse(string Token, string RefreshToken);
        public record LoginRequest(string Username, string Email, string Password);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/login",
                async (LoginRequest request,
                IRequestHandler<LoginCommand, LoginCommandResult> handler,
                CancellationToken cancellationToken) =>
            {
                var query = request.Adapt<LoginCommand>();
                var result = await handler.Handle(query, cancellationToken);
                var response = result.Adapt<LoginResponse>();

                return Results.Ok(response);
            })
            .WithName("Login")
            .WithTags("Identity")
            .Produces<LoginCommandResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithOpenApi(operation =>
            {
                operation.RequestBody = DefaultRequestProvider.RegisterRequest();
                return operation;
            });
        }
    }
}
