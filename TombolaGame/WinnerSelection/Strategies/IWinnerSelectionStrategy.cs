using TombolaGame.Models;

namespace TombolaGame.WinnerSelection.Strategies
{
    public interface IWinnerSelectionStrategy
    {
        Player SelectWinner(IEnumerable<Player> players, IEnumerable<Player> winners);
    }
}
