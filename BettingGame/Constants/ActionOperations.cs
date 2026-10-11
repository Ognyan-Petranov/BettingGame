
namespace BettingGame.Constants
{
    public static class ActionOperations
    {
        public const string Deposit = "deposit";
        public const string Withdraw = "withdraw";
        public const string Bet = "bet";
        public const string Exit = "exit";
        public static readonly HashSet<string> AllowedOperations =
        [
            Deposit,
            Withdraw,
            Bet,
            Exit
        ];
    }
}
