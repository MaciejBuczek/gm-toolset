using Common.Messaging.Exceptions;
using Communication.API.Exceptions;

namespace Communication.API.Unit.Tests.Services
{
    public class CommunicationServiceEmailSenderTests
    {
        private readonly string _mailTo = "target@email.com";
        private readonly string _subject = "subject";
        private readonly string _htmlContent = "<h1>content</h1>";
        private readonly Mock<IOptions<AzureCommunicationService>> _optionsProvider = new();
        private readonly Mock<EmailClient> _emailClient = new();
        private readonly AzureCommunicationService _options = new()
        {
            SenderAddress = "testsender@email.com",
            ConnectionString = ""
        };

        private CommunicationServiceEmailSender GetSender()
        {
            _optionsProvider.Setup(op => op.Value).Returns(_options);
            _emailClient.Setup(e => e.SendAsync(WaitUntil.Completed, It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EmailSendOperation("test-id", Mock.Of<EmailClient>()));
            return new CommunicationServiceEmailSender(_emailClient.Object, _optionsProvider.Object);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldSendEmailWhenDataIsValid()
        {
            //Arrange
            var sender = GetSender();

            //Act
            await sender.SendEmailAsync(_mailTo, _subject, _htmlContent);

            //Assert
            _emailClient.Verify(e => e.SendAsync(WaitUntil.Completed, It.Is<EmailMessage>(m =>
            
                m.Content.Subject == _subject &&
                m.Content.Html == _htmlContent &&
                m.SenderAddress == _options.SenderAddress &&
                m.Recipients.To.Any(x => x.Address == _mailTo)
            ), It.IsAny<CancellationToken>()), Times.Once);
            
        }

        [Fact]
        public async Task SenEmailAsyncShouldThrowEmailContentExceptionIfTargetEmailIsMissing()
        {
            //Arrange
            var sender = GetSender();

            //Act, Assert
            await Assert.ThrowsAsync<EmailContentException>(async () => await sender.SendEmailAsync(string.Empty, _subject, _htmlContent));
        }

        [Fact]
        public async Task SenEmailAsyncShouldThrowEmailContentExceptionIfSubjectIsMissing()
        {
            //Arrange
            var sender = GetSender();

            //Act, Assert
            await Assert.ThrowsAsync<EmailContentException>(async () => await sender.SendEmailAsync(_mailTo, string.Empty, _htmlContent));
        }

        [Fact]
        public async Task SenEmailAsyncShouldThrowRetryableExceptionIfInnerSenderThrowsValidRequestFailedException()
        {
            //Arrange
            _emailClient.Setup(e => e.SendAsync(WaitUntil.Completed, It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(408, "timeout"));
            _optionsProvider.Setup(op => op.Value).Returns(_options);
            var sender = new CommunicationServiceEmailSender(_emailClient.Object, _optionsProvider.Object);

            //Act, Assert
            await Assert.ThrowsAsync<RetryableException>(async () => await sender.SendEmailAsync(_mailTo, _subject, _htmlContent));
        }

        [Fact]
        public async Task SenEmailAsyncShouldThrowRequestFailedExceptionIfInnerSenderThrowsInvalidRequestFailedException()
        {
            //Arrange
            _emailClient.Setup(e => e.SendAsync(WaitUntil.Completed, It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(400, "bad-request"));
            _optionsProvider.Setup(op => op.Value).Returns(_options);
            var sender = new CommunicationServiceEmailSender(_emailClient.Object, _optionsProvider.Object);

            //Act, Assert
            await Assert.ThrowsAsync<RequestFailedException>(async () => await sender.SendEmailAsync(_mailTo, _subject, _htmlContent));
        }

        [Fact]
        public async Task SenEmailAsyncShouldThrowAnyExceptionIfInnerSenderThrowsNonRequestFailedException()
        {
            //Arrange
            _emailClient.Setup(e => e.SendAsync(WaitUntil.Completed, It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            _optionsProvider.Setup(op => op.Value).Returns(_options);
            var sender = new CommunicationServiceEmailSender(_emailClient.Object, _optionsProvider.Object);

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await sender.SendEmailAsync(_mailTo, _subject, _htmlContent));
        }
    }
}
