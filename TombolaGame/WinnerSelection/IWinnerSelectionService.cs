using TombolaGame.Models;

namespace TombolaGame.WinnerSelection;

public interface IWinnerSelectionService
{
    Task<IEnumerable<TombolaWinner>> DrawWinnersAsync(Tombola tombola);
}