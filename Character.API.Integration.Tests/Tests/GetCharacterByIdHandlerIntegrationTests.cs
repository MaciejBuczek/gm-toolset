namespace Character.API.Integration.Tests.Tests
{
    public class GetCharacterByIdHandlerIntegrationTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<IQueryHandler<GetCharacterByIdQuery, GetCharacterByIdResult>>(Factory)
    {
        private readonly static Guid _characterId = Guid.NewGuid();
        private readonly GetCharacterByIdQuery _query = new(Id: _characterId);
        private readonly CharacterEntity _character = new()
        {
            Id = _characterId,
            UserId = Guid.NewGuid(),
            SchemaId = Guid.NewGuid(),
            Name = "Glimbo the Goblin",
            Description = "He is just a wibble gobwin that doesn't drop good items. Pleas don't swing your sword at him, he will die in just one attack",
            Statistics =
            [
                new Statistic
                {
                    Name = "Hit points",
                    Value = "3"
                },
                new Statistic
                {
                    Name = "Size",
                    Value = "Wibble"
                }
            ]
        };

        [Fact]
        public async Task Handle_ShouldThrowCharacterNotFoundIfCharacterIsNotFoundInDatabase()
        {
            //Arrange, Act, Assert
            await Assert.ThrowsAsync<CharacterNotFoundException>(async () => await handler.Handle(_query));
        }

        [Fact]
        public async Task Handle_ShouldReturnCharacterFromDatabase()
        {
            //Arrange, Act
            await repository.CreateCharacterAsync(_character, default);
            var result = await handler.Handle(_query);

            //Assert
            Assert.NotNull(result);

            Assert.Equal(result.Character.Id, _character.Id);
            Assert.Equal(result.Character.UserId, _character.UserId);
            Assert.Equal(result.Character.SchemaId, _character.SchemaId);
            Assert.Equal(result.Character.Name, _character.Name);
            Assert.Equal(result.Character.Description, _character.Description);
            Assert.Equal(result.Character.Statistics.Count, _character.Statistics.Count);

            Assert.Equal(
                _character.Statistics.Select(x => x.Name),
                result.Character.Statistics.Select(x => x.Name));
            Assert.Equal(
                _character.Statistics.Select(x => x.Value),
                result.Character.Statistics.Select(x => x.Value));
        }
    }
}
