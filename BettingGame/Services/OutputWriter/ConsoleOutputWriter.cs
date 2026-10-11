
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

        public void InvalidInputFormatMessage()
        {
            Console.WriteLine("Invalid input format. Please enter operation followed by transaction amount.");
        }

        public void InvalidTransactionAmountMessage()
        {
            Console.WriteLine("Please enter a valid transaction amount.");
        }

        public void PublishGoodbyeMessage()
        {
            Console.WriteLine("Thank you for playing! Hope to see you again soon.");
        }

        public void SubmitActionMessage()
        {
            Console.WriteLine("Please, submit action (or type 'exit' to quit):");
        }

        public void WithdrawSuccessMessage(string currency, decimal amount, decimal balance)
        {
            Console.WriteLine($"Your withdrawal of {currency}{amount} was successful. Your current balance is: {currency}{balance}");
        }
    }
}
