namespace Character.API.Features.GetCharactersBySchemaId
{
    public record GetCharactersBySchemaIdResult(IEnumerable<Entities.Character> Characters);
    public record GetCharactersBySchemaIdQuery(Guid Id) : IQuery<GetCharactersBySchemaIdResult>;

    public class GetCharactersBySchemaIdQueryHandler(ICharacterRepository repository) : IQueryHandler<GetCharactersBySchemaIdQuery, GetCharactersBySchemaIdResult>
    {
        public async Task<GetCharactersBySchemaIdResult> Handle(GetCharactersBySchemaIdQuery query, CancellationToken cancellationToken = default)
        {
            var characters = await repository.GetCharacterBySchemaIdAsync(query.Id, cancellationToken);
            return characters.IsEmpty()
                ? throw new CharacterNotFoundException($"Characters not found - SchemaId: {query.Id}")
                : new GetCharactersBySchemaIdResult(characters);
        }
    }
}
