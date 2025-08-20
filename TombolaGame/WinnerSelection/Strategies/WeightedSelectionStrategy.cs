using TombolaGame.Models;
using TombolaGame.WinnerSelection.Strategies;

namespace TombolaGame.WinnerSelector.Strategies
{
    public class WeightedSelectionStrategy : IWinnerSelectionStrategy
    {
        private readonly Random _random = new();

        public Player SelectWinner(IEnumerable<Player> players, IEnumerable<Player> winners)
        {
            var weightedList = new List<Player>();

            foreach (var player in players)
            {
                for (int i = 0; i < player.Weight; i++)
                {
                    weightedList.Add(player);
                }
            }

            if (!weightedList.Any())
                throw new InvalidOperationException("No players available for weighted selection.");

            return weightedList[_random.Next(weightedList.Count)];
        }
    }
}