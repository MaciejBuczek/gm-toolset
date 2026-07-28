namespace Identity.API.Features.Register
{
    public class RegisterEndpoint : ICarterModule
    {
        public record RegisterResponse(bool Succeded);
        public record RegisterRequest(string Username, string Email, string Password);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", 
                async (RegisterRequest request, 
                IRequestHandler<RegisterCommand, RegisterCommandResult> handler,
                CancellationToken cancellationToken) =>
            {
                var command = request.Adapt<RegisterCommand>();
                var result = await handler.Handle(command, cancellationToken);
                var response = result.Adapt<RegisterResponse>();

                return Results.Created();
            })
            .WithName("Register")
            .WithTags("Identity")
            .Produces<RegisterCommandResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();
        }
    }
}
