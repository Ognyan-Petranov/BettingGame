
using System.Numerics;

namespace BettingGame.Services.OutputWriter
{
    public class ConsoleOutputWriter : IConsoleOutputWriter
    {
        public void BetLostMessage(string currency, decimal balance)
        {
            Console.WriteLine($"No luck this time! Your current balance is: {currency}{balance}");
        }

        public void BetWonMessage(string currency, decimal amountWon, decimal balance)
        {
            Console.WriteLine($"Congrats - you won {currency}{amountWon}! Your current balance is: {currency}{balance}");
        }

        public void CustomMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DepositSuccessMessage(string currency, decimal amount, decimal balance)
        {
            Console.WriteLine($"Your deposit of {currency}{amount} was successful. Your current balance is: {currency}{balance}");
        }

        public void InsufficientFundsForWithdrawalMessage(string currency, decimal amount, decimal balance)
        {
            Console.WriteLine($"Insufficient funds for withdrawal of {currency}{amount}. Your current balance is: {currency}{balance}");
        }

        public void WithdrawSuccessMessage(string currency, decimal amount, decimal balance)
        {
            Console.WriteLine($"Your withdrawal of {currency}{amount} was successful. Your current balance is: {currency}{balance}");
        }
    }
}
