namespace Character.API.UnitTests.Features
{
    public class GetCharactersBySchemaIdQueryTests
    {
        private readonly Mock<ICharacterRepository> _repository = new();  

        [Fact]
        public async Task Handle_ShouldThrowCharacterNotFoundException_WhenSchemaIdIsNotMatching()
        {
            //Arrange
            var query = new GetCharactersBySchemaIdQuery(Guid.NewGuid());

            _repository.Setup(x => x.GetCharacterBySchemaIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => []);

            var handler = new GetCharactersBySchemaIdQueryHandler(_repository.Object);

            //Act & Assert
            await Assert.ThrowsAsync<CharacterNotFoundException>(async () => await handler.Handle(query, default));
        }

        [Fact]
        public async Task Handle_ShouldReturnGetCharactersBySchemaIdResult_WhenSchemaIdIsMatching()
        {
            //Arrange
            var schemaId = Guid.NewGuid();
            var query = new GetCharactersBySchemaIdQuery(schemaId);

            _repository.Setup(x => x.GetCharacterBySchemaIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([
                    new Entities.Character
                    {
                        Id = Guid.NewGuid(),
                        SchemaId = schemaId,
                        UserId = Guid.NewGuid(),
                        Name = "Glimbo the Goblin",
                        Description = "He is just a wibble goblin",
                        Statistics = [new Statistic { Name = "Size", Value = "Wibble" }]
                    },
                    new Entities.Character
                    {
                        Id = Guid.NewGuid(),
                        SchemaId = schemaId,
                        UserId = Guid.NewGuid(),
                        Name = "Glorbo the Goblin",
                        Description = "He is a big and strong goblin",
                        Statistics = [new Statistic { Name = "Size", Value = "Massive" }]
                    }]);

            var handler = new GetCharactersBySchemaIdQueryHandler(_repository.Object);

            //Act & Assert
            var response = await handler.Handle(query, default);
            Assert.NotNull(response?.Characters);
            Assert.Equal(2, response.Characters.Count());
            Assert.All(response.Characters, c => c.SchemaId.Equals(schemaId));
        }
    }
}
