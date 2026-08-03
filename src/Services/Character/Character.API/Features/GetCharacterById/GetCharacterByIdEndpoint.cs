namespace Character.API.Features.GetCharacterById
{
    public class GetCharacterByIdEndpoint : ICarterModule
    {
        public record GetCharacterByIdResponse(Entities.Character Character);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/characters/{id}",
                async (Guid id,
                IRequestHandler<GetCharacterByIdQuery,
                GetCharacterByIdResult> handler,
                CancellationToken cancellationToken) =>
                {
                    var command = new GetCharacterByIdQuery(id);
                    var result = await handler.Handle(command, cancellationToken);
                    var response = result.Adapt<GetCharacterByIdResponse>();

                    return Results.Ok(response);
                })
                .WithName("GetCharacterById")
                .WithTags("Character")
                .RequireAuthorization()
                .Produces<CreateCharacterResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .WithOpenApi();
        }
    }
}
