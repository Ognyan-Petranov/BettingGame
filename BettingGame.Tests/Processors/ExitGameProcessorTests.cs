
using BettingGame.Domain.Entities.User;
using BettingGame.Processors;
using BettingGame.Services.OutputWriter;
using Moq;

namespace BettingGame.Tests.Processors
{
    public class ExitGameProcessorTests
    {
        private readonly Mock<IConsoleOutputWriter> _consoleOutputWriterMock;

        public ExitGameProcessorTests()
        {
            _consoleOutputWriterMock = new Mock<IConsoleOutputWriter>();
        }

        [Fact]
        public void Process_ShouldCallExitGameMessage_WhenCalled()
        {
            // Arrange
            var exitGameProcessor = CreateServiceToTest();
            // Act
            exitGameProcessor.Process(It.IsAny<User>(), It.IsAny<decimal>());
            // Assert
            _consoleOutputWriterMock.Verify(x => x.PublishGoodbyeMessage(), Times.Once);
        }

        private ExitGameProcessor CreateServiceToTest()
        {
            return new ExitGameProcessor(_consoleOutputWriterMock.Object);
        }
    }
}
