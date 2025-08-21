using TombolaGame.Enums;
using TombolaGame.Exceptions;
using TombolaGame.Helpers;
using TombolaGame.Models;
using TombolaGame.Models.Mappers;
using TombolaGame.Repositories;
using TombolaGame.Repositories.Contracts;
using TombolaGame.WinnerSelection;

namespace TombolaGame.Services
{
    public class TombolaService : ITombolaService
    {
        private readonly ITombolaRepository _tombolaRepository;
        private readonly IAwardRepository _awardRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IWinnerSelectionService _winnerSelectionService;

        public TombolaService(ITombolaRepository tombolaRepository, IAwardRepository awardRepository, IPlayerRepository playerRepository,IWinnerSelectionService winnerSelectionService)
        {
            _tombolaRepository = tombolaRepository;
            _awardRepository = awardRepository;
            _playerRepository = playerRepository;
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
            ValidateTombolaRequest(request);

            var tombola = ModelMapper.ToTombola(request);

            var createdTombola = await _tombolaRepository.AddAsync(tombola);
            return ModelMapper.ToResponse(createdTombola);
        }

        public async Task<TombolaResponse> UpdateTombolaAsync(int tombolaId, TombolaRequest request)
        {
            ValidateTombolaRequest(request);

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

            if (tombola.State != TombolaState.Waiting)
                throw new InvalidOperationException("Tombola is not in 'Waiting' state.");

            if (tombola.Players.Count >= tombola.MaximumPlayers)
                throw new InvalidOperationException("Tombola is full.");

            var player = await _playerRepository.GetByNameAsync(playerName);
            if (player == null)
            {
                throw new InvalidOperationException("Player does not exist.");
            }

            if (tombola.Players.Any(p => p.Id == player.Id))
                throw new InvalidOperationException("Player is already joined in this tombola.");

            tombola.Players.Add(player);
            await _tombolaRepository.UpdateAsync(tombola);

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

            if (tombola.State != TombolaState.Waiting)
                throw new InvalidOperationException($"Cannot assign award. Current state is '{tombola.State}'.");

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

            if (tombola.State != TombolaState.Waiting)
                throw new InvalidOperationException($"Cannot start tombola. Current state is '{tombola.State}'.");

            if (tombola.Players.Count < tombola.MinimumPlayers)
                throw new InvalidOperationException("Cannot start tombola. Not enough players.");

            if (tombola.Awards.Count < tombola.MinimumAwards)
                throw new InvalidOperationException("Cannot start tombola. Not enough awards.");

            tombola.State = TombolaState.InProgress;
            await _tombolaRepository.UpdateAsync(tombola);

            tombola.Winners = (await _winnerSelectionService.DrawWinnersAsync(tombola)).ToList();

            tombola.State = TombolaState.Finished;
            await _tombolaRepository.UpdateAsync(tombola);
        }

        private void ValidateTombolaRequest(TombolaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("Name is required.");

            if (request.MinPlayers < 1)
                throw new ValidationException("Minimum players must be at least 1.");

            if (request.MaxPlayers < request.MinPlayers)
                throw new ValidationException("Maximum players cannot be less than minimum players.");

            if (request.MinAwards < 1)
                throw new ValidationException("Minimum awards must be at least 1.");

            if (request.MaxAwards < request.MinAwards)
                throw new ValidationException("Maximum awards cannot be less than minimum awards.");

            if (request.StrategyType != null && !Enum.IsDefined(typeof(StrategyType), request.StrategyType))
                throw new ValidationException("Invalid strategy type.");
        }
    }
}