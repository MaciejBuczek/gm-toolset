namespace Character.API.UnitTests.Features
{
    public class GetCharactersByUserIdHandlerTests
    {
        private readonly Mock<ICharacterRepository> _repository = new();

        [Fact]
        public async Task Handle_ShouldThrowCharacterNotFoundException_WhenUserIdIsNotMatching()
        {
            //Arrange
            var query = new GetCharactersByUserIdQuery(Guid.NewGuid());

            _repository.Setup(x => x.GetCharacterByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => []);

            var handler = new GetCharactersByUserIdQueryHandler(_repository.Object);

            //Act & Assert
            await Assert.ThrowsAsync<CharacterNotFoundException>(async () => await handler.Handle(query, default));
        }

        [Fact]
        public async Task Handle_ShouldReturnGetCharactersBySchemaIdResult_WhenUserIdIsMatching()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var query = new GetCharactersByUserIdQuery(userId);

            _repository.Setup(x => x.GetCharacterByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([
                    new Entities.Character
                    {
                        Id = Guid.NewGuid(),
                        SchemaId = Guid.NewGuid(),
                        UserId = userId,
                        Name = "Glimbo the Goblin",
                        Description = "He is just a wibble goblin",
                        Statistics = [new Statistic { Name = "Size", Value = "Wibble" }]
                    },
                    new Entities.Character
                    {
                        Id = Guid.NewGuid(),
                        SchemaId = Guid.NewGuid(),
                        UserId = userId,
                        Name = "Glorbo the Goblin",
                        Description = "He is a big and strong goblin",
                        Statistics = [new Statistic { Name = "Size", Value = "Massive" }]
                    }]);

            var handler = new GetCharactersByUserIdQueryHandler(_repository.Object);

            //Act & Assert
            var response = await handler.Handle(query, default);
            Assert.NotNull(response?.Characters);
            Assert.Equal(2, response.Characters.Count());
            Assert.All(response.Characters, c => c.UserId.Equals(userId));
        }
    }
}
