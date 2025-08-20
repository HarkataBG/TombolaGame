using TombolaGame.Models;
using TombolaGame.WinnerSelection.Strategies;

namespace TombolaGame.WinnerSelector.Strategies
{
    public class RandomSelectionStrategy : IWinnerSelectionStrategy
    {
        private readonly Random _random = new();

        public Player SelectWinner(IEnumerable<Player> players, IEnumerable<Player> winners)
        {
            var availablePlayers = players.ToList();

            if (!availablePlayers.Any())
                throw new InvalidOperationException("No players available for selection.");

            return availablePlayers[_random.Next(availablePlayers.Count)];
        }
    }
}