using TombolaGame.Models.Mappers;
using TombolaGame.Models;
using TombolaGame.Enums;

namespace TombolaGame.Helpers
{
    public static class ModelMapper
    {
        public static Tombola ToTombola(TombolaRequest request)
        {
            return new Tombola
            {
                Name = request.Name,
                MinimumPlayers = request.MinPlayers,
                MaximumPlayers = request.MaxPlayers,
                MinimumAwards = request.MinAwards,
                MaximumAwards = request.MaxAwards,
                StrategyType = request.StrategyType ?? StrategyType.OnePrizePerPlayer
            };
        }

        public static TombolaResponse ToResponse(Tombola tombola)
        {
            return new TombolaResponse
            {
                Id = tombola.Id,
                Name = tombola.Name,
                State = tombola.State,
                StrategyType = tombola.StrategyType,

                MinimumPlayers = tombola.MinimumPlayers,
                MaximumPlayers = tombola.MaximumPlayers,
                MinimumAwards = tombola.MinimumAwards,
                MaximumAwards = tombola.MaximumAwards,

                PlayerNames = tombola.Players.Select(p => p.Name).ToList(),
                AwardNames = tombola.Awards.Select(a => a.Name).ToList(),
                WinnerNames = tombola.Winners
                    .Select(w => w.Player.Name)
                    .ToList()
            };
        }

        public static void UpdateTombolaFromRequest(Tombola existing, TombolaRequest request)
        {
            existing.Name = request.Name ?? existing.Name;
            existing.MinimumPlayers = request.MinPlayers;
            existing.MaximumPlayers = request.MaxPlayers;
            existing.MinimumAwards = request.MinAwards;
            existing.MaximumAwards = request.MaxAwards;
            existing.StrategyType = request.StrategyType ?? existing.StrategyType;
        }

        public static AwardResponse ToResponse(Award award)
        {
            return new AwardResponse
            {
                Id = award.Id,
                Name = award.Name,
                TombolaId = award.TombolaId
            };
        }

        public static PlayerResponse ToResponse(Player player)
        {
            return new PlayerResponse
            {
                Id = player.Id,
                Name = player.Name,
                Weight = player.Weight,
                TombolaIds = player.Tombolas.Select(t => t.Id).ToList()
            };
        }
    }
}
