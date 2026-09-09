namespace Character.API.Integration.Tests.Tests
{
    public class UpdateCharacterHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<ICommandHandler<UpdateCharacterCommand, UpdateCharaterResult>>(Factory)
    {
        private readonly Guid _characterId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _schemaId = Guid.NewGuid();

        private CharacterEntity GetCharacter()
        {
            return new()
            {
                Id = _characterId,
                UserId = _userId,
                SchemaId = _schemaId,
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
        }

        [Fact]
        public async Task Handle_ShouldThrowCharacterNotFoundIfCharactersAreNotFoundInDatabase()
        {
            //Arrange
            var character = GetCharacter();
            var command = new UpdateCharacterCommand(character.Id, character.UserId, character.SchemaId, character.Name, character.Description, character.Statistics);

            // Act, Assert
            await Assert.ThrowsAsync<CharacterNotFoundException>(async () => await handler.Handle(command));
        }

        [Fact]
        public async Task Handle_ShouldUpdateCharacterInDatabase()
        {
            //Arrange
            var characterOld = GetCharacter();
            var characterNew = GetCharacter();
            characterNew.Name = "New Name";
            characterNew.Description = "New Description";
            characterNew.Statistics = [new Statistic { Name = "New Stat Name", Value = "New Stat Value" }];

            var command = new UpdateCharacterCommand(characterNew.Id, characterNew.UserId, characterNew.SchemaId, characterNew.Name, characterNew.Description, characterNew.Statistics);
            await repository.CreateCharacterAsync(characterOld, default);

            //Act
            var result = await handler.Handle(command);
            var dbResult = await repository.GetCharacterByIdAsync(_characterId, default);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Success);

            Assert.NotNull(dbResult);
            Assert.Equal(dbResult.Id, characterNew.Id);
            Assert.Equal(dbResult.UserId, characterNew.UserId);
            Assert.Equal(dbResult.SchemaId, characterNew.SchemaId);
            Assert.Equal(dbResult.Name, characterNew.Name);
            Assert.Equal(dbResult.Description, characterNew.Description);
            Assert.Equal(dbResult.Statistics.Count, characterNew.Statistics.Count);

            Assert.Equal(
                characterNew.Statistics.Select(x => x.Name),
                dbResult.Statistics.Select(x => x.Name));
            Assert.Equal(
                characterNew.Statistics.Select(x => x.Value),
                dbResult.Statistics.Select(x => x.Value));
        }
    }
}
