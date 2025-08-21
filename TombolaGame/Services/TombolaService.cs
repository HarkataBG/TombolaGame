using TombolaGame.Exceptions;
using TombolaGame.Helpers;
using TombolaGame.Models;
using TombolaGame.Models.Mappers;
using TombolaGame.Repositories;
using TombolaGame.WinnerSelection;

namespace TombolaGame.Services
{
    public class TombolaService : ITombolaService
    {
        private readonly ITombolaRepository _tombolaRepository;
        private readonly IAwardRepository _awardRepository;
        private readonly IWinnerSelectionService _winnerSelectionService;

        public TombolaService(ITombolaRepository tombolaRepository, IAwardRepository awardRepository, IWinnerSelectionService winnerSelectionService)
        {
            _tombolaRepository = tombolaRepository;
            _awardRepository = awardRepository;
            _winnerSelectionService = winnerSelectionService;
        }

        public async Task<IEnumerable<TombolaResponse>> GetAllTombolasAsync()
        {
            var tombolas = await _tombolaRepository.GetAllAsync();
            return tombolas.Select(ModelMapper.ToResponse);
        }

        public async Task<TombolaResponse> GetTombolaByIdAsync(int tombolaId)
        {
            var tombola = await _tombolaRepository.GetByIdAsync(tombolaId);

            if (tombola == null)
                throw new EntityNotFoundException("Tombola", tombolaId);

            return ModelMapper.ToResponse(tombola);
        }

        public async Task<TombolaResponse> CreateTombolaAsync(TombolaRequest request)
        {
            var tombola = ModelMapper.ToTombola(request);            

            var createdTombola = await _tombolaRepository.AddAsync(tombola);
            return ModelMapper.ToResponse(createdTombola);
        }

        public async Task<TombolaResponse> UpdateTombolaAsync(int tombolaId, TombolaRequest request)
        {
            var existingTombola = await _tombolaRepository.GetByIdAsync(tombolaId);
            if (existingTombola == null)
                throw new EntityNotFoundException("Tombola", tombolaId);

            ModelMapper.UpdateTombolaFromRequest(existingTombola, request);
            await _tombolaRepository.UpdateAsync(existingTombola);

            return ModelMapper.ToResponse(existingTombola);
        }

        public async Task DeleteTombolaAsync(int tombolaId)
        {
            var existingTombola = await _tombolaRepository.GetByIdAsync(tombolaId);
            if (existingTombola == null)
                throw new EntityNotFoundException("Tombola", tombolaId);

            await _tombolaRepository.DeleteAsync(existingTombola);
        }

        public async Task JoinTombolaAsync(int tombolaId, string playerName)
        {
            var tombola = await _tombolaRepository.GetByIdAsync(tombolaId);
            if (tombola == null)
                throw new EntityNotFoundException("Tombola", tombolaId);

            if (tombola.State != "Waiting")
                throw new InvalidOperationException("Tombola is not in 'Waiting' state.");

            if (tombola.Players.Count >= tombola.MaximumPlayers)
                throw new InvalidOperationException("Tombola is full.");

            var player = new Player { Name = playerName };
            tombola.Players.Add(player);

            await _tombolaRepository.UpdateAsync(tombola);

            if (tombola.Players.Count == tombola.MaximumPlayers &&
                tombola.Awards.Count >= tombola.MinimumAwards)
            {
                await StartTombolaAsync(tombola.Id);
            }
        }

        public async Task AssignAwardAsync(int tombolaId, int awardId)
        {
            var tombola = await _tombolaRepository.GetByIdAsync(tombolaId)
                ?? throw new EntityNotFoundException("Tombola", tombolaId);

            if (tombola.State != "Waiting")
                throw new InvalidOperationException("Cannot assign award. Tombola is not in 'Waiting' state.");

            var award = await _awardRepository.GetByIdAsync(awardId)
                ?? throw new EntityNotFoundException("Award", awardId);

            if (award.TombolaId != null && award.TombolaId != tombolaId)
                throw new InvalidOperationException("Award is already assigned to another tombola.");

            award.TombolaId = tombola.Id;
            await _awardRepository.UpdateAsync(award);
        }

        public async Task StartTombolaAsync(int tombolaId)
        {
            var tombola = await _tombolaRepository.GetByIdAsync(tombolaId);
            if (tombola == null)
                throw new EntityNotFoundException("Tombola", tombolaId);

            if (tombola.Players.Count < tombola.MinimumPlayers)
                throw new InvalidOperationException("Cannot start tombola. Not enough players.");

            if (tombola.Awards.Count < tombola.MinimumAwards)
                throw new InvalidOperationException("Cannot start tombola. Not enough awards.");

            tombola.State = "Active";
            await _tombolaRepository.UpdateAsync(tombola);

            tombola.Winners = (await _winnerSelectionService.DrawWinnersAsync(tombola)).ToList();

            tombola.State = "Completed";
            await _tombolaRepository.UpdateAsync(tombola);
        }
    }
}