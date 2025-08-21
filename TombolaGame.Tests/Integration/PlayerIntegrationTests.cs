using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TombolaGame.Models.Mappers;

namespace TombolaGame.Tests.Integration
{
    public class PlayerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;


        public PlayerIntegrationTests()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                    });
                });

            _client = factory.CreateClient();
        }


        [Fact]
        public async Task Can_Create_And_Get_Player()
        {
            PlayerResponse? created = null;

            try
            {
                var uniquePlayerName = "TestName_IT_Create_Player_" + Guid.NewGuid().ToString("N");

                var request = new PlayerRequest { Name = uniquePlayerName, Weight = 1 };

                var createResponse = await _client.PostAsJsonAsync("/api/players", request);
                createResponse.EnsureSuccessStatusCode();


                created = await createResponse.Content.ReadFromJsonAsync<PlayerResponse>();
                created.Should().NotBeNull();
                created!.Name.Should().Be(uniquePlayerName);


                var getResponse = await _client.GetAsync($"/api/players/{created.Id}");
                getResponse.EnsureSuccessStatusCode();

                var fetched = await getResponse.Content.ReadFromJsonAsync<PlayerResponse>();
                fetched!.Id.Should().Be(created.Id);

            }
            finally
            {
                if (created is not null)
                    await _client.DeleteAsync($"/api/players/{created.Id}");

            }
        }

    }
}

