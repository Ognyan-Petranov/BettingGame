
using BettingGame.Requests;
using BettingGame.Services.Helpers;

namespace BettingGame.Tests.Services.Helpers
{
    public class ActionRequestParserTests
    {
        [Theory]
        [InlineData("deposit 100", "deposit", 100)]
        [InlineData("withdraw 50", "withdraw", 50)]
        [InlineData("bet 10", "bet", 10)]
        [InlineData("exit", "exit", 0)]
        public void ParseActionRequest_WhenCalledWithValidInput_ReturnsExpectedResult(string input, string operation, decimal amount)
        {
            // Arrange
            var parser = new ActionRequestParser();

            // Act
            var result = parser.TryParse(input, out ActionRequest? actionRequest, out string? errorMessage);

            // Assert
            Assert.True(result);
            Assert.Equal(operation, actionRequest?.Operation);
            Assert.Equal(amount, actionRequest?.Amount);
            Assert.Null(errorMessage);
        }

        [Theory]
        [InlineData("invalid input", "Invalid transaction amount.")]
        [InlineData("deposit abc", "Invalid transaction amount.")]
        [InlineData("withdraw", "Invalid input format. Please enter operation followed by transaction amount.")]
        [InlineData("bet", "Invalid input format. Please enter operation followed by transaction amount.")]
        [InlineData("", "Invalid input format. Please enter operation followed by transaction amount.")]
        public void ParseActionRequest_WhenCalledWithInvalidInput_ReturnsFalse(string input, string expectedErrorMessage)
        {
            // Arrange
            var parser = new ActionRequestParser();

            // Act
            var result = parser.TryParse(input, out ActionRequest? actionRequest, out string? errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(expectedErrorMessage, errorMessage);
            Assert.Null(actionRequest);
        }
    }
}   
