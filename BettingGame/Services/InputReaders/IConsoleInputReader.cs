
using BettingGame.Requests;

namespace BettingGame.Services.InputReaders
{
    public interface IConsoleInputReader
    {
        ActionRequest ReadInput();
    }
}
