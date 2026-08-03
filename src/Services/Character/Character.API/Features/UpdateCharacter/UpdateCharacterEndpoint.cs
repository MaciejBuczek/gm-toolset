namespace Character.API.Features.UpdateCharacter
{
    public record UpdateCharacterResponse(bool Success);
    public record UpdateCharacterRequest(Guid Id, Guid UserId, Guid SchemaId, string Name, string Description, ICollection<Statistic> Statistics);

    public class UpdateCharacterEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/characters",
                async (UpdateCharacterRequest request,
                IRequestHandler<UpdateCharacterCommand,
                UpdateCharaterResult> handler,
                CancellationToken cancellationToken) =>
                {
                    var command = request.Adapt<UpdateCharacterCommand>();
                    var result = await handler.Handle(command, cancellationToken);
                    var response = result.Adapt<UpdateCharacterResponse>();

                    return Results.Ok(response);
                })
                .WithName("UpdateCharacter")
                .WithTags("Character")
                .RequireAuthorization()
                .Produces<CreateCharacterResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .WithOpenApi();
        }
    }
}
