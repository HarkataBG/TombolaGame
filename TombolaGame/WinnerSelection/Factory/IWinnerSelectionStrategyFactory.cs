using TombolaGame.WinnerSelection.Strategies;

public interface IWinnerSelectionStrategyFactory
{
    IWinnerSelectionStrategy GetStrategy(string strategyType);
}