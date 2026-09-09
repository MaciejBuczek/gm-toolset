namespace Character.API.Integration.Tests.Tests
{
    public class DeleteCharacterCommandHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<ICommandHandler<DeleteCharacterByIdCommand, DeleteCharacterByIdResult>>(Factory)
    {
        private static readonly Guid _characterId = Guid.NewGuid();
        private readonly DeleteCharacterByIdCommand _command = new(Id: _characterId);

        private readonly CharacterEntity _character = new()
        {
            Id = _characterId,
            Name = "Test Character",
            Statistics = [new Statistic() { Name = "Test Stat Name", Value = "Test Stat Value" }]
        };

        [Fact]
        public async Task Handle_ShouldThrowCharacterNotFoundExceptionWhenCharacterIsNotFoundInDatabase()
        {
            //Arrange, Act, Assert
            await Assert.ThrowsAsync<CharacterNotFoundException>(async () => await handler.Handle(_command));
        }

        [Fact]
        public async Task Handle_ShouldRemoveCharacterFromDatabase()
        {
            //Arrange
            await repository.CreateCharacterAsync(_character, default);

            //Act
            await handler.Handle(_command);
            var foundCharacter = await repository.GetCharacterByIdAsync(_characterId, default);

            //Assert
            Assert.Null(foundCharacter);
        }
    }
}
