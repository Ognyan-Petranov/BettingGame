
using BettingGame.Constants;
using BettingGame.Requests;
using BettingGame.Services.Helpers;
using BettingGame.Services.InputReaders;
using BettingGame.Services.OutputWriter;
using BettingGame.Validators;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace BettingGame.Tests.Services
{
    public class ConsoleInputReaderTests
    {
        private readonly Mock<IConsoleOutputWriter> _consoleOutputWriterMock;
        private readonly Mock<IActionRequestParser> _actionRequestParserMock;
        private readonly IValidator<ActionRequest> _actionRequestValidator;

        public ConsoleInputReaderTests()
        {
            _consoleOutputWriterMock = new Mock<IConsoleOutputWriter>();
            _actionRequestParserMock = new Mock<IActionRequestParser>();
            _actionRequestValidator = new ActionRequestValidator();
        }

        [Theory]
        [InlineData("bet 5", "bet", 5)]
        [InlineData("deposit 50", "deposit", 50)]
        [InlineData("withdraw 25", "withdraw", 25)]
        [InlineData("exit", "exit", 0)]
        public void ReadActionRequest_ShouldReturnValidActionRequest_WhenInputIsValid(string input, string operation, decimal amount)
        {
            // Arrange
            var originalInput = Console.In;

            ActionRequest? requestToReturn = new ActionRequest() { Amount = amount, Operation = operation }; ;
            string? errorToReturn = null;
            Mock.Get(_actionRequestParserMock.Object).Setup(x => x.TryParse(input, out requestToReturn, out errorToReturn)).Returns(true);

            try
            {
                using var stringReader = new StringReader(input);
                Console.SetIn(stringReader);

                var consoleInputReader = new ConsoleInputReader(
                    _consoleOutputWriterMock.Object,
                    _actionRequestValidator,
                    _actionRequestParserMock.Object);

                // Act
                var actionRequest = consoleInputReader.ReadInput();

                // Assert
                Assert.Equal(operation, actionRequest.Operation);
                Assert.Equal(amount, actionRequest.Amount);
            }
            finally
            {
                Console.SetIn(originalInput);
            }
        }

        [Theory]
        [InlineData("deposit -50\ndeposit 10", 10, "deposit", "Amount must be greater than zero.")]
        [InlineData("withdraw -50\nwithdraw 10", 10, "withdraw", "Amount must be greater than zero.")]
        [InlineData("bet -50\nbet 10", 10, "bet", "Amount must be greater than zero.")]
        [InlineData("something 100\ndeposit 10", 10, "deposit", "Invalid operation. Please enter 'deposit', 'withdraw', 'bet', or 'exit'.")]
        [InlineData("bet 100\nbet 10", 10, "bet", "Please enter a valid bet amount between 1 and 10.")]
        public void ReadActionRequest_ShouldReturnValidActionRequest_WhenInputIsInvalidThenValid(string input, decimal validAmount, string operation, string expectedErrorMessage)
        {
            // Arrange
            var originalInput = Console.In;
            var serviceToTest = new ConsoleInputReader(_consoleOutputWriterMock.Object, new ActionRequestValidator(), new ActionRequestParser());

            try
            {
                using var stringReader = new StringReader(input);
                Console.SetIn(stringReader);
                
                // Act
                var actionRequest = serviceToTest.ReadInput();
                // Assert
                Assert.Equal(operation, actionRequest.Operation);
                Assert.Equal(validAmount, actionRequest.Amount);
                _consoleOutputWriterMock.Verify(x => x.CustomMessage(expectedErrorMessage), Times.Once);
            }
            finally
            {
                Console.SetIn(originalInput);
            }
        }

        [Fact]
        public void ReadActionRequest_ShouldReturnExitRequest_WhenInputIsExit()
        {
            // Arrange
            var originalInput = Console.In;
            var serviceToTest = new ConsoleInputReader(_consoleOutputWriterMock.Object, new ActionRequestValidator(), new ActionRequestParser());

            try
            {
                using var stringReader = new StringReader(ActionOperations.Exit);
                Console.SetIn(stringReader);

                // Act
                var actionRequest = serviceToTest.ReadInput();

                // Assert
                Assert.Equal(ActionOperations.Exit, actionRequest.Operation);
            }
            finally
            {
                Console.SetIn(originalInput);
            }
        }
    }
}
