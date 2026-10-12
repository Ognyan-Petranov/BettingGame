
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
        private readonly User userToTest;
        private readonly Mock<IConsoleOutputWriter> _mockConsoleOutputWriter;
        private readonly Mock<IBetRewardCalculator> _mockBetRewardCalculator;

        public BetActionProcessorTests()
        {
            userToTest = new User("sample@mail.com", "Jon", "Doe");
            userToTest.Wallet.AddAmount(BetAmounts.MaximumBetAmount);

            _mockConsoleOutputWriter = new Mock<IConsoleOutputWriter>();
            _mockBetRewardCalculator = new Mock<IBetRewardCalculator>();
        }

        [Fact]
        public void ProcessBetAction_WhenCalled_CalculatesCorrectRewardResult()
        {
            // Arrange
            var betActionProcessor = new BetActionProcessor(_mockConsoleOutputWriter.Object, _mockBetRewardCalculator.Object);
            Mock.Get(_mockBetRewardCalculator.Object).Setup(x => x.CalculateReward(It.IsAny<decimal>())).Returns<decimal>(x => x * RewardMultipliers.BaseRewardMultiplier);
            decimal initialBalance = userToTest.Wallet.Balance;
            decimal amountWon = initialBalance * RewardMultipliers.BaseRewardMultiplier;
            // Act
            betActionProcessor.Process(userToTest, BetAmounts.MaximumBetAmount);

            // Assert
            Assert.Equal(amountWon, userToTest.Wallet.Balance);
            Assert.NotEqual(initialBalance, userToTest.Wallet.Balance);
            Assert.True(initialBalance < userToTest.Wallet.Balance);
            _mockConsoleOutputWriter.Verify(x => x.BetWonMessage(userToTest.Wallet.Currency, amountWon, userToTest.Wallet.Balance), Times.Once);
        }

        [Fact]
        public void ProcessBetAction_WhenCalledWithInsufficientBalance_ThrowsInvalidOperationException()
        {
            // Arrange
            var betActionProcessor = new BetActionProcessor(_mockConsoleOutputWriter.Object, _mockBetRewardCalculator.Object);
            userToTest.Wallet.SubtractAmount(BetAmounts.MaximumBetAmount); // Set balance to 0

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => betActionProcessor.Process(userToTest, BetAmounts.MaximumBetAmount));
            Assert.Equal("Insufficient balance for the bet. Please deposit more funds.", exception.Message);
        }

        [Fact]
        public void ProcessBetAction_WhenCalledWithNoReward_CallsBetLostMessage()
        {
            // Arrange
            var betActionProcessor = new BetActionProcessor(_mockConsoleOutputWriter.Object, _mockBetRewardCalculator.Object);
            Mock.Get(_mockBetRewardCalculator.Object).Setup(x => x.CalculateReward(It.IsAny<decimal>())).Returns(decimal.Zero);
            // Act
            betActionProcessor.Process(userToTest, BetAmounts.MaximumBetAmount);
            // Assert
            _mockConsoleOutputWriter.Verify(x => x.BetLostMessage(userToTest.Wallet.Currency, userToTest.Wallet.Balance), Times.Once);
            Assert.Equal(decimal.Zero, userToTest.Wallet.Balance);
        }
    }
}
