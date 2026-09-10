namespace Communication.API.Unit.Tests.Services
{
    public class RazorRendererTests
    {
        private readonly Mock<IRazorViewEngine> _razoViewEngineMock = new();
        private readonly Mock<IServiceProvider> _serviceProviderMock = new();
        private readonly Mock<ITempDataProvider> _tempDataProviderMock = new();

        [Fact]
        public async Task RenderTemplateAsync_ShouldThrowInvalidOperationExceptionWhenTemplateIsNotFound()
        {
            // Arrange

            _razoViewEngineMock.Setup(x => x.GetView(null,It.IsAny<string>(),true)).Returns(ViewEngineResult.NotFound("Template not found", ["locations..."]));
            var renderer = new RazorRenderer(_serviceProviderMock.Object, _razoViewEngineMock.Object, _tempDataProviderMock.Object);

            // Act & Assert
             await Assert.ThrowsAsync<InvalidOperationException>(() =>renderer.RenderTemplateAsync("template",new object()));
        }
    }
}
