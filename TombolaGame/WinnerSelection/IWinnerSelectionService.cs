using TombolaGame.Models;

namespace TombolaGame.WinnerSelection;

public interface IWinnerSelectionService
{
    Task<IEnumerable<Player>> DrawWinnersAsync(Tombola tombola);
    Task<Player?> DrawWinnerAsync(Tombola tombola);
}