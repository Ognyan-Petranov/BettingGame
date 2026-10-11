
namespace BettingGame.Requests
{
    public class ActionRequest
    {
        public string Operation { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
