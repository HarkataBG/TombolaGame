using TombolaGame.Enums;
using TombolaGame.WinnerSelection.Strategies;

public interface IWinnerSelectionStrategyFactory
{
    IWinnerSelectionStrategy GetStrategy(StrategyType strategyType);
}