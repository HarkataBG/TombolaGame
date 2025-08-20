using TombolaGame.WinnerSelection.Strategies;
using TombolaGame.WinnerSelector.Strategies;

public class WinnerSelectionStrategyFactory : IWinnerSelectionStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WinnerSelectionStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWinnerSelectionStrategy GetStrategy(string strategyType)
    {
        return strategyType switch
        {
            "Weighted" => _serviceProvider.GetRequiredService<WeightedSelectionStrategy>(),
            "Random" => _serviceProvider.GetRequiredService<RandomSelectionStrategy>(),
            _ => _serviceProvider.GetRequiredService<OnePrizePerPlayerStrategy>()
        };
    }
}