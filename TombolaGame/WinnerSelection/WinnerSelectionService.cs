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

        public async Task<IEnumerable<Player>> DrawWinnersAsync(Tombola tombola)
        {
            var winners = new List<Player>();
            var strategy = _strategyFactory.GetStrategy(tombola.StrategyType);

            foreach (var award in tombola.Awards)
            {
                var winner = strategy.SelectWinner(tombola.Players, tombola.Winners);
                if (winner == null) break;

                winners.Add(winner);

                await _publishEndpoint.Publish(new WinnerSelectedEvent(
                    tombola.Id,
                    winner.Id,
                    winner.Name
                ));
            }

            return winners;
        }      
    }
}