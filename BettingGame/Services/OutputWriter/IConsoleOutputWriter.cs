
namespace BettingGame.Services.OutputWriter
{
    public interface IConsoleOutputWriter
    {
        void PublishGoodbyeMessage();
        void BetLostMessage(string currency, decimal balance);
        void BetWonMessage(string currency, decimal amountWon, decimal balance);
        void DepositSuccessMessage(string currency, decimal amount, decimal balance);
        void InsufficientFundsForWithdrawalMessage(string currency, decimal amount, decimal balance);
        void WithdrawSuccessMessage(string currency, decimal amount, decimal balance);
        void CustomMessage(string message);
        void SubmitActionMessage();
        void InvalidInputFormatMessage();
        void InvalidTransactionAmountMessage();
    }
}
