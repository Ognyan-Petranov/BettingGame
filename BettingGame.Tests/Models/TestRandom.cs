
namespace BettingGame.Tests.Models
{
    internal sealed class TestRandom : Random
    {
        private readonly int _outcome;
        private readonly double _randomDouble;

        public TestRandom(int outcome, double randomDouble = 0)
        {
            _outcome = outcome;
            _randomDouble = randomDouble;
        }

        public override int Next(int minValue, int maxValue)
            => _outcome;

        public override double NextDouble()
            => _randomDouble;
    }
}
