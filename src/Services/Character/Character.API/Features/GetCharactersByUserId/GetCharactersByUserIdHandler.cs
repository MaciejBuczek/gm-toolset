namespace Character.API.Features.GetCharactersByUserId
{
    public record GetCharactersByUserIdResult(IEnumerable<Entities.Character> Characters);
    public record GetCharactersByUserIdQuery(Guid Id) : IQuery<GetCharactersByUserIdResult>;

    public class GetCharactersByUserIdQueryHandler(ICharacterRepository repository) : IQueryHandler<GetCharactersByUserIdQuery, GetCharactersByUserIdResult>
    {
        public async Task<GetCharactersByUserIdResult> Handle(GetCharactersByUserIdQuery query, CancellationToken cancellationToken = default)
        {
            var characters = await repository.GetCharacterByUserIdAsync(query.Id, cancellationToken);
            return characters.IsEmpty()
                ? throw new CharacterNotFoundException($"Characters not found - UserId: {query.Id}")
                : new GetCharactersByUserIdResult(characters);
        }
    }
}
