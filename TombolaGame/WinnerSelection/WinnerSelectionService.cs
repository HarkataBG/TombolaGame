using MassTransit;
using TombolaGame.Events;
using TombolaGame.Models;
using TombolaGame.WinnerSelection;

namespace TombolaGame.Services
{
    public class WinnerSelectionService : IWinnerSelectionService
    {
        private readonly IWinnerSelectionStrategyFactory _strategyFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public WinnerSelectionService(IWinnerSelectionStrategyFactory strategyFactory, IPublishEndpoint publishEndpoint)
        {
            _strategyFactory = strategyFactory;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IEnumerable<TombolaWinner>> DrawWinnersAsync(Tombola tombola)
        {
            var winners = new List<TombolaWinner>();
            var strategy = _strategyFactory.GetStrategy(tombola.StrategyType);

            foreach (var award in tombola.Awards)
            {
                var player = strategy.SelectWinner(tombola.Players, tombola.Winners.Select(w => w.Player).ToList());
                if (player == null) break;

                winners.Add(new TombolaWinner
                {
                    TombolaId = tombola.Id,
                    PlayerId = player.Id
                });

                await _publishEndpoint.Publish(new WinnerSelectedEvent(
                    tombola.Id,
                    player.Id,
                    player.Name
                ));
            }

            return winners;
        }
    }
}