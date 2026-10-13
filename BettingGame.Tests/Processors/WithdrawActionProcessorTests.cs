
using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Processors;
using BettingGame.Services.OutputWriter;
using Moq;

namespace BettingGame.Tests.Processors
{
    public class WithdrawActionProcessorTests
    {
        private readonly Mock<IConsoleOutputWriter> _consoleOutputWriterMock;
        private readonly User _userToTest;


        public WithdrawActionProcessorTests()
        {
            _consoleOutputWriterMock = new Mock<IConsoleOutputWriter>();
            _userToTest = new User("sample@mail.com", "Jon", "Doe");
            _userToTest.Wallet.AddAmount(BetAmounts.MaximumBetAmount);
        }

        [Fact]
        public void ProcessAction_ShouldReturnCorrectResult_WhenValidWithdrawAmount()
        {
            // Arrange
            var processor = CreateServiceToTest();
            decimal withdrawAmount = BetAmounts.MaximumBetAmount;
            decimal initialBalance = _userToTest.Wallet.Balance;
            // Act
            processor.Process(_userToTest, withdrawAmount);

            // Assert
            Assert.Equal(initialBalance - withdrawAmount, _userToTest.Wallet.Balance);
            _consoleOutputWriterMock.Verify(x => x.WithdrawSuccessMessage(_userToTest.Wallet.Currency, withdrawAmount, _userToTest.Wallet.Balance), Times.Once);
        }

        [Fact]
        public void ProcessAction_ShouldThrowException_WhenInsufficientBalance()
        {
            // Arrange
            var processor = CreateServiceToTest();
            decimal withdrawAmount = BetAmounts.MaximumBetAmount + 1;

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => processor.Process(_userToTest, withdrawAmount));
            Assert.Equal($"Insufficient funds for withdrawal of {_userToTest.Wallet.Currency}{withdrawAmount}. Your current balance is: {_userToTest.Wallet.Currency}{_userToTest.Wallet.Balance}", exception.Message);
        }

        [Fact]
        public void ProcessAction_ShouldThrowException_WhenInvalidWithdrawAmount()
        {
            // Arrange
            var processor = CreateServiceToTest();
            decimal withdrawAmount = -1;

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => processor.Process(_userToTest, withdrawAmount));
            Assert.Equal($"Withdrawal amount must be greater than zero. You entered: {_userToTest.Wallet.Currency}{withdrawAmount}", exception.Message);
        }

        private WithdrawActionProcessor CreateServiceToTest()
        {
            return new WithdrawActionProcessor(_consoleOutputWriterMock.Object);
        }
    }
}
