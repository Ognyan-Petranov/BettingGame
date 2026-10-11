using BettingGame.Requests;

namespace BettingGame.Services.Helpers
{
    public interface IActionRequestParser
    {
        bool TryParse(string input, out ActionRequest? request, out string? errorMessage);
    }
}
