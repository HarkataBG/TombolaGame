using FluentAssertions;
using Moq;
using TombolaGame.Exceptions;
using TombolaGame.Models;
using TombolaGame.Repositories;

namespace TombolaGame.Tests.Services
{
    public class AwardServiceTests
    {
        private readonly Mock<IAwardRepository> _awardRepositoryMock;
        private readonly AwardService _awardService;

        public AwardServiceTests()
        {
            _awardRepositoryMock = new Mock<IAwardRepository>();
            _awardService = new AwardService(_awardRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAwardsAsync_ShouldReturnAllAwards()
        {
            // Arrange
            var awards = new List<Award>
            {
                new Award { Id = 1, Name = "Award 1" },
                new Award { Id = 2, Name = "Award 2" }
            };

            _awardRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(awards);

            // Act
            var result = await _awardService.GetAllAwardsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Select(r => r.Name).Should().Contain(new[] { "Award 1", "Award 2" });
        }

        [Fact]
        public async Task GetAwardByIdAsync_ShouldReturnAward_WhenExists()
        {
            // Arrange
            var award = new Award { Id = 1, Name = "Prize" };
            _awardRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(award);

            // Act
            var result = await _awardService.GetAwardByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Prize");
        }

        [Fact]
        public async Task GetAwardByIdAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            _awardRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Award?)null);

            // Act
            Func<Task> act = async () => await _awardService.GetAwardByIdAsync(1);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Award with identifier '1' was not found.");
        }

        [Fact]
        public async Task CreateAwardAsync_ShouldCreateAward()
        {
            // Arrange
            var request = new AwardRequest { Name = "New Award" };

            // Act
            var result = await _awardService.CreateAwardAsync(request);

            // Assert
            result.Name.Should().Be("New Award");
            _awardRepositoryMock.Verify(r => r.AddAsync(It.Is<Award>(a => a.Name == "New Award")), Times.Once);
        }

        [Fact]
        public async Task UpdateAwardAsync_ShouldUpdateAward_WhenExists()
        {
            // Arrange
            var award = new Award { Id = 1, Name = "Old Name" };
            var request = new AwardRequest { Name = "Updated Name" };

            _awardRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(award);

            // Act
            var result = await _awardService.UpdateAwardAsync(1, request);

            // Assert
            result.Name.Should().Be("Updated Name");
            _awardRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Award>(a => a.Name == "Updated Name")), Times.Once);
        }

        [Fact]
        public async Task UpdateAwardAsync_ShouldThrow_WhenAwardNotFound()
        {
            // Arrange
            _awardRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Award?)null);

            var request = new AwardRequest { Name = "New Name" };

            // Act
            Func<Task> act = async () => await _awardService.UpdateAwardAsync(1, request);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Award with identifier '1' was not found.");
        }

        [Fact]
        public async Task DeleteAwardAsync_ShouldDeleteAward_WhenNotAssigned()
        {
            // Arrange
            var award = new Award { Id = 1, Name = "Award", TombolaId = null };

            _awardRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(award);

            // Act
            await _awardService.DeleteAwardAsync(1);

            // Assert
            _awardRepositoryMock.Verify(r => r.DeleteAsync(award), Times.Once);
        }

        [Fact]
        public async Task DeleteAwardAsync_ShouldThrow_WhenAssignedToTombola()
        {
            // Arrange
            var award = new Award { Id = 1, TombolaId = 100 };

            _awardRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(award);

            // Act
            Func<Task> act = async () => await _awardService.DeleteAwardAsync(1);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot delete award assigned to a tombola.");
        }

        [Fact]
        public async Task DeleteAwardAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            _awardRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Award?)null);

            // Act
            Func<Task> act = async () => await _awardService.DeleteAwardAsync(1);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Award with identifier '1' was not found.");
        }
    }
}
