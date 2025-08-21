using Moq;
using FluentAssertions;
using TombolaGame.Services;
using TombolaGame.Repositories.Contracts;
using TombolaGame.Models;
using TombolaGame.Enums;
using TombolaGame.Exceptions;
using TombolaGame.WinnerSelection;
using TombolaGame.Models.Mappers;
using TombolaGame.Repositories;

namespace TombolaGame.Tests.Services
{
    public class TombolaServiceTests
    {
        private readonly Mock<ITombolaRepository> _tombolaRepoMock;
        private readonly Mock<IAwardRepository> _awardRepoMock;
        private readonly Mock<IPlayerRepository> _playerRepoMock;
        private readonly Mock<IWinnerSelectionService> _winnerSelectionMock;
        private readonly TombolaService _service;

        public TombolaServiceTests()
        {
            _tombolaRepoMock = new Mock<ITombolaRepository>();
            _awardRepoMock = new Mock<IAwardRepository>();
            _playerRepoMock = new Mock<IPlayerRepository>();
            _winnerSelectionMock = new Mock<IWinnerSelectionService>();

            _service = new TombolaService(
                _tombolaRepoMock.Object,
                _awardRepoMock.Object,
                _playerRepoMock.Object,
                _winnerSelectionMock.Object
            );
        }

        [Fact]
        public async Task GetTombolaByIdAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            _tombolaRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tombola?)null);

            // Act
            var act = async () => await _service.GetTombolaByIdAsync(1);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Tombola with identifier '1' was not found.");
        }

        [Fact]
        public async Task CreateTombolaAsync_ShouldThrow_WhenNameMissing()
        {
            var request = new TombolaRequest
            {
                Name = "",
                MinPlayers = 1,
                MaxPlayers = 2,
                MinAwards = 1,
                MaxAwards = 2,
                StrategyType = StrategyType.OnePrizePerPlayer
            };

            Func<Task> act = async () => await _service.CreateTombolaAsync(request);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Name is required.");
        }

        [Fact]
        public async Task JoinTombolaAsync_ShouldThrow_WhenTombolaNotFound()
        {
            _tombolaRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tombola?)null);

            Func<Task> act = async () => await _service.JoinTombolaAsync(1, "player1");

            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task JoinTombolaAsync_ShouldThrow_WhenFull()
        {
            var tombola = new Tombola
            {
                Id = 1,
                State = TombolaState.Waiting,
                MaximumPlayers = 2,
                MinimumAwards = 1,
                Players = new List<Player>
                {
                    new Player { Id = 1, Name = "P1" },
                    new Player { Id = 2, Name = "P2" }
                },
                Awards = new List<Award> { new Award { Id = 1 } }
            };

            _tombolaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tombola);

            Func<Task> act = async () => await _service.JoinTombolaAsync(1, "newPlayer");

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Tombola is full.");
        }

       
    }
}
