using TombolaGame.Enums;
using TombolaGame.WinnerSelection.Strategies;
using TombolaGame.WinnerSelector.Strategies;

public class WinnerSelectionStrategyFactory : IWinnerSelectionStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WinnerSelectionStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWinnerSelectionStrategy GetStrategy(StrategyType strategyType)
    {
        return strategyType switch
        {
            StrategyType.WeightedSelection => _serviceProvider.GetRequiredService<WeightedSelectionStrategy>(),
            StrategyType.RandomSelection => _serviceProvider.GetRequiredService<RandomSelectionStrategy>(),
            _ => _serviceProvider.GetRequiredService<OnePrizePerPlayerStrategy>()
        };
    }
}