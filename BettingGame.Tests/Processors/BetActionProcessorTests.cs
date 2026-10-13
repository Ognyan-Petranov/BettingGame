
using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Processors;
using BettingGame.Services.BetRewardCalculator;
using BettingGame.Services.OutputWriter;
using Moq;

namespace BettingGame.Tests.Processors
{
    public class BetActionProcessorTests
    {
        private readonly User _userToTest;
        private readonly Mock<IConsoleOutputWriter> _mockConsoleOutputWriter;
        private readonly Mock<IBetRewardCalculator> _mockBetRewardCalculator;

        public BetActionProcessorTests()
        {
            _userToTest = new User("sample@mail.com", "Jon", "Doe");
            _userToTest.Wallet.AddAmount(BetAmounts.MaximumBetAmount);

            _mockConsoleOutputWriter = new Mock<IConsoleOutputWriter>();
            _mockBetRewardCalculator = new Mock<IBetRewardCalculator>();
        }

        [Fact]
        public void ProcessBetAction_WhenCalled_CalculatesCorrectRewardResult()
        {
            // Arrange
            var betActionProcessor = CreateServiceToTest();
            Mock.Get(_mockBetRewardCalculator.Object).Setup(x => x.CalculateReward(It.IsAny<decimal>())).Returns<decimal>(x => x * RewardMultipliers.BaseRewardMultiplier);
            decimal initialBalance = _userToTest.Wallet.Balance;
            decimal amountWon = Math.Round(initialBalance * RewardMultipliers.BaseRewardMultiplier, 2);
            // Act
            betActionProcessor.Process(_userToTest, BetAmounts.MaximumBetAmount);

            // Assert
            Assert.Equal(amountWon, _userToTest.Wallet.Balance);
            Assert.NotEqual(initialBalance, _userToTest.Wallet.Balance);
            Assert.True(initialBalance < _userToTest.Wallet.Balance);
            _mockConsoleOutputWriter.Verify(x => x.BetWonMessage(_userToTest.Wallet.Currency, amountWon, _userToTest.Wallet.Balance), Times.Once);
        }

        [Fact]
        public void ProcessBetAction_WhenCalledWithInsufficientBalance_ThrowsInvalidOperationException()
        {
            // Arrange
            var betActionProcessor = CreateServiceToTest();
            _userToTest.Wallet.SubtractAmount(BetAmounts.MaximumBetAmount); // Set balance to 0

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => betActionProcessor.Process(_userToTest, BetAmounts.MaximumBetAmount));
            Assert.Equal("Insufficient balance for the bet. Please deposit more funds.", exception.Message);
        }

        [Fact]
        public void ProcessBetAction_WhenCalledWithNoReward_CallsBetLostMessage()
        {
            BetActionProcessor betActionProcessor = CreateServiceToTest();
            Mock.Get(_mockBetRewardCalculator.Object).Setup(x => x.CalculateReward(It.IsAny<decimal>())).Returns(decimal.Zero);
            // Act
            betActionProcessor.Process(_userToTest, BetAmounts.MaximumBetAmount);
            // Assert
            _mockConsoleOutputWriter.Verify(x => x.BetLostMessage(_userToTest.Wallet.Currency, _userToTest.Wallet.Balance), Times.Once);
            Assert.Equal(decimal.Zero, _userToTest.Wallet.Balance);
        }

        private BetActionProcessor CreateServiceToTest()
        {
            return new BetActionProcessor(_mockConsoleOutputWriter.Object, _mockBetRewardCalculator.Object);
        }
    }
}
