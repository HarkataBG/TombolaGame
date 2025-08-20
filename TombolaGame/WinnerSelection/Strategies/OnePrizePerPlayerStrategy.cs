using TombolaGame.Models;
using TombolaGame.WinnerSelection.Strategies;

namespace TombolaGame.WinnerSelector.Strategies
{
    public class OnePrizePerPlayerStrategy : IWinnerSelectionStrategy
    {
        private readonly Random _random = new();

        public Player SelectWinner(IEnumerable<Player> players, IEnumerable<Player> winners)
        {
            var availablePlayers = players.Except(winners).ToList();
            if (!availablePlayers.Any())
                throw new InvalidOperationException("No available players left.");

            return availablePlayers[_random.Next(availablePlayers.Count)];
        }
    }
}
