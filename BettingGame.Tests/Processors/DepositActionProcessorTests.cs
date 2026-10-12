
using BettingGame.Domain.Entities.User;
using BettingGame.Processors;
using BettingGame.Services.OutputWriter;
using Moq;

namespace BettingGame.Tests.Processors
{
    public class DepositActionProcessorTests
    {
        private readonly Mock<IConsoleOutputWriter> _mockConsoleOutputWriter;
        private readonly User _userToTest;

        public DepositActionProcessorTests()
        {
            _userToTest = new User("sample@mail.com", "Jon", "Doe");
            _mockConsoleOutputWriter = new Mock<IConsoleOutputWriter>();
        }

        [Fact]
        public void Process_ShouldUpdateUserBalance_WhenDepositIsValid()
        {
            // Arrange
            var depositAmount = 50m;
            var depositProcessor = CreateServiceToTest();

            // Act
            depositProcessor.Process(_userToTest, depositAmount);

            // Assert
            Assert.Equal(depositAmount, _userToTest.Wallet.Balance);
            _mockConsoleOutputWriter.Verify(x => x.DepositSuccessMessage(_userToTest.Wallet.Currency, depositAmount, _userToTest.Wallet.Balance), Times.Once);
        }

        [Fact]
        public void Process_ThrowsException_WhenDepositIsInvalid()
        {
            // Arrange
            var depositAmount = -50m;
            var depositProcessor = CreateServiceToTest();
            decimal initialBalance = _userToTest.Wallet.Balance;

            // Assert & Act
            var exception = Assert.Throws<InvalidOperationException>(() => depositProcessor.Process(_userToTest, depositAmount));
            Assert.Equal("Deposit amount must be greater than zero.", exception.Message);
            Assert.Equal(initialBalance, _userToTest.Wallet.Balance);
            _mockConsoleOutputWriter.Verify(x => x.DepositSuccessMessage(_userToTest.Wallet.Currency, depositAmount, It.IsAny<decimal>()), Times.Never);
        }

        private DepositActionProcessor CreateServiceToTest()
        {
            return new DepositActionProcessor(_mockConsoleOutputWriter.Object);
        }
    }
}
