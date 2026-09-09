using Common.Exceptions;

namespace Character.API.Integration.Tests.Tests
{
    public class CreateCharacterHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<ICommandHandler<CreateCharacterCommand, CreateCharacterResult>>(Factory)
    {
        private readonly CreateCharacterCommand _command = new(
            UserId: Guid.NewGuid(),
            SchemaId: Guid.NewGuid(),
            Name: "Glimbo the Goblin",
            Description: "He is just a wibble gobwin that doesn't drop good items. Pleas don't swing your sword at him, he will die in just one attack",
            Statistics:
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
            ]);

        [Fact]
        public async Task Handle_CreateShouldAddCharacterToDatabase()
        {
            //Arrange
            var command = _command;

            //Act
            var result = await handler.Handle(command);
            var dbResult = await repository.GetCharacterByIdAsync(result.CharacterId, default);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(dbResult);

            Assert.Equal(result.CharacterId, dbResult.Id);
            Assert.Equal(_command.UserId, dbResult.UserId);
            Assert.Equal(_command.SchemaId, dbResult.SchemaId);
            Assert.Equal(_command.Name, dbResult.Name);
            Assert.Equal(_command.Description, dbResult.Description);
            Assert.Equal(command.Statistics.Count, dbResult.Statistics.Count);

            Assert.Equal(
                command.Statistics.Select(x => x.Name),
                dbResult.Statistics.Select(x => x.Name));
            Assert.Equal(
                command.Statistics.Select(x => x.Value),
                dbResult.Statistics.Select(x => x.Value));
        }
    }
}
