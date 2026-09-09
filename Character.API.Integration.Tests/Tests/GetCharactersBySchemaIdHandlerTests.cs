using Character.API.Entities;
using FastExpressionCompiler;

namespace Character.API.Integration.Tests.Tests
{
    public class GetCharactersBySchemaIdHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<IQueryHandler<GetCharactersBySchemaIdQuery, GetCharactersBySchemaIdResult>>(Factory)
    {
        private readonly static Guid _schemaId = Guid.NewGuid();
        private readonly GetCharactersBySchemaIdQuery _query = new(Id: _schemaId);
        private readonly CharacterEntity[] _characters = [
            new CharacterEntity()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
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
            },
            new CharacterEntity()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                SchemaId = _schemaId,
                Name = "Glombo the Ork",
                Description = "Big and strong",
                Statistics =
                [
                    new Statistic
                    {
                        Name = "Hit points",
                        Value = "20"
                    },
                    new Statistic
                    {
                        Name = "Size",
                        Value = "Big (and also strong)"
                    }
                ]
            }
        ];

        [Fact]
        public async Task Handle_ShouldThrowCharacterNotFoundIfCharactersAreNotFoundInDatabase()
        {
            //Arrange, Act, Assert
            await Assert.ThrowsAsync<CharacterNotFoundException>(async () => await handler.Handle(_query));
        }

        [Fact]
        public async Task Handle_ShouldReturnAllCharactersWithMatchingSchemaIdFromDatabase()
        {
            //Arrange
            foreach(var character in _characters)
            {
                await repository.CreateCharacterAsync(character, default);
            }

            //Act
            var result = await handler.Handle(_query);
            var resultArray = result.Characters.AsArray();

            //Assert
            Assert.NotNull(result?.Characters);
            Assert.Equal(resultArray.Length, _characters.Length);

            for(var i = 0; i < resultArray.Length; i++)
            {
                Assert.Equal(resultArray[i].Id, _characters[i].Id);
                Assert.Equal(resultArray[i].UserId, _characters[i].UserId);
                Assert.Equal(resultArray[i].SchemaId, _characters[i].SchemaId);
                Assert.Equal(resultArray[i].Name, _characters[i].Name);
                Assert.Equal(resultArray[i].Description, _characters[i].Description);
                Assert.Equal(resultArray[i].Statistics.Count, _characters[i].Statistics.Count);

                Assert.Equal(
                    _characters[i].Statistics.Select(x => x.Name),
                    resultArray[i].Statistics.Select(x => x.Name));
                Assert.Equal(
                    _characters[i].Statistics.Select(x => x.Value),
                    resultArray[i].Statistics.Select(x => x.Value));
            }
        }
    }
}
