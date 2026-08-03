namespace Character.API.Features.DeleteCharacterById
{
    public class DeleteCharacterByIdEndpoint : ICarterModule
    {
        public record DeleteCharacterByIdResponse(bool Success);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/characters/{id}",
                async (Guid id,
                IRequestHandler<DeleteCharacterByIdCommand,
                DeleteCharacterByIdResult> handler,
                CancellationToken cancellationToken) =>
                {
                    var command = new DeleteCharacterByIdCommand(id);
                    var result = await handler.Handle(command, cancellationToken);
                    var response = result.Adapt<DeleteCharacterByIdResponse>();

                    return Results.Ok(response);
                })
                .WithName("DeleteCharacterById")
                .WithTags("Character")
                .RequireAuthorization()
                .Produces<DeleteCharacterByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .WithOpenApi();
        }
    }
}
