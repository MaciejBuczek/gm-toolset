namespace Character.API.Features.GetCharactersBySchemaId
{
    public record GetCharactersBySchemaIdResponse(IEnumerable<Entities.Character> Characters);

    public class GetCharactersBySchemaIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/characters/schema/{id}",
                async (Guid id,
                IRequestHandler<GetCharactersBySchemaIdQuery,
                GetCharactersBySchemaIdResult> handler,
                CancellationToken cancellationToken) =>
                {
                    var command = new GetCharactersBySchemaIdQuery(id);
                    var result = await handler.Handle(command, cancellationToken);
                    var response = result.Adapt<GetCharactersBySchemaIdResponse>();

                    return Results.Ok(response);
                })
                .WithName("GetCharacterBySchemaId")
                .WithTags("Character")
                .RequireAuthorization()
                .Produces<CreateCharacterResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .WithOpenApi();
        }
    }
}
