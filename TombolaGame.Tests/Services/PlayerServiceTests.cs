using FluentAssertions;
using Moq;
using TombolaGame.Exceptions;
using TombolaGame.Models;
using TombolaGame.Models.Mappers;
using TombolaGame.Repositories.Contracts;
using TombolaGame.Services;

namespace TombolaGame.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _playerService = new PlayerService(_playerRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllPlayersAsync_ShouldReturnMappedResponses()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { Id = 1, Name = "Alice", Weight = 10 },
                new Player { Id = 2, Name = "Bob", Weight = 20 }
            };
            _playerRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(players);

            // Act
            var result = await _playerService.GetAllPlayersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Alice");
        }

        [Fact]
        public async Task GetPlayerByIdAsync_ShouldReturnPlayer_WhenFound()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Alice", Weight = 10 };
            _playerRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);

            // Act
            var result = await _playerService.GetPlayerByIdAsync(1);

            // Assert
            result.Id.Should().Be(1);
            result.Name.Should().Be("Alice");
        }

        [Fact]
        public async Task GetPlayerByIdAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            _playerRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Player?)null);

            // Act
            Func<Task> act = async () => await _playerService.GetPlayerByIdAsync(99);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("*Player*");
        }

        [Fact]
        public async Task CreatePlayerAsync_ShouldCreatePlayer_WhenValid()
        {
            // Arrange
            var request = new PlayerRequest { Name = "Alice", Weight = 10 };
            _playerRepositoryMock.Setup(r => r.GetByNameAsync("Alice")).ReturnsAsync((Player?)null);

            // Act
            var result = await _playerService.CreatePlayerAsync(request);

            // Assert
            result.Name.Should().Be("Alice");
            _playerRepositoryMock.Verify(r => r.AddAsync(It.Is<Player>(p => p.Name == "Alice")), Times.Once);
        }

        [Fact]
        public async Task CreatePlayerAsync_ShouldThrow_WhenPlayerExists()
        {
            // Arrange
            var request = new PlayerRequest { Name = "Alice", Weight = 10 };
            _playerRepositoryMock.Setup(r => r.GetByNameAsync("Alice")).ReturnsAsync(new Player { Id = 1, Name = "Alice" });

            // Act
            Func<Task> act = async () => await _playerService.CreatePlayerAsync(request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Player with name 'Alice' already exists.");
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldUpdatePlayer_WhenValid()
        {
            // Arrange
            var request = new PlayerRequest { Name = "Alice Updated", Weight = 15 };
            var player = new Player { Id = 1, Name = "Alice", Weight = 10 };

            _playerRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);
            _playerRepositoryMock.Setup(r => r.GetByNameAsync("Alice Updated")).ReturnsAsync((Player?)null);

            // Act
            var result = await _playerService.UpdatePlayerAsync(1, request);

            // Assert
            result.Name.Should().Be("Alice Updated");
            result.Weight.Should().Be(15);
            _playerRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Player>(p => p.Id == 1 && p.Name == "Alice Updated")), Times.Once);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldThrow_WhenPlayerNotFound()
        {
            // Arrange
            _playerRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Player?)null);
            var request = new PlayerRequest { Name = "Ghost", Weight = 10 };

            // Act
            Func<Task> act = async () => await _playerService.UpdatePlayerAsync(99, request);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("*Player*");
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldThrow_WhenAnotherPlayerWithSameNameExists()
        {
            // Arrange
            var request = new PlayerRequest { Name = "Bob", Weight = 20 };
            var existing = new Player { Id = 1, Name = "Alice", Weight = 10 };
            var conflicting = new Player { Id = 2, Name = "Bob", Weight = 20 };

            _playerRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _playerRepositoryMock.Setup(r => r.GetByNameAsync("Bob")).ReturnsAsync(conflicting);

            // Act
            Func<Task> act = async () => await _playerService.UpdatePlayerAsync(1, request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Another player with name 'Bob' already exists.");
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldDelete_WhenPlayerExists()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Alice" };
            _playerRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);

            // Act
            await _playerService.DeletePlayerAsync(1);

            // Assert
            _playerRepositoryMock.Verify(r => r.DeleteAsync(player), Times.Once);
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            _playerRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Player?)null);

            // Act
            Func<Task> act = async () => await _playerService.DeletePlayerAsync(99);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("*Player*");
        }
    }
}