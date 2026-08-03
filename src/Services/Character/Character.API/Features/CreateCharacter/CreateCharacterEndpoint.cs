namespace Character.API.Features.CreateCharacter
{
    public record CreateCharacterResponse(Guid CharacterId);
    public record CreateCharacterRequest(Guid UserId, Guid SchemaId, string Name, string Description, ICollection<Statistic> Statistics);

    public class CreateCharacterEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/characters", 
                async (CreateCharacterRequest request,
                IRequestHandler<CreateCharacterCommand,
                CreateCharacterResult> handler,
                CancellationToken cancellationToken) =>
                {
                    var command = request.Adapt<CreateCharacterCommand>();
                    var result = await handler.Handle(command, cancellationToken);
                    var response = result.Adapt<CreateCharacterResponse>();

                    return Results.Created($"/characters{response.CharacterId}", response);
                })
                .WithName("CreateCharacter")
                .WithTags("Character")
                .RequireAuthorization()
                .Produces<CreateCharacterResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .WithOpenApi(operation =>
                {
                    operation.RequestBody = DefaultRequestProvider.CreateCharacterRequest();
                    return operation;
                });
        }
    }
}
