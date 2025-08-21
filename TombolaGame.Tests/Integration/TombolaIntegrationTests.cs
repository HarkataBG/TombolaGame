using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using TombolaGame.Models.Mappers;

namespace TombolaGame.Tests.Integration
{
    public class TombolaIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;


        public TombolaIntegrationTests()
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
        public async Task Can_Create_And_Get_Tombola()
        {
            TombolaResponse? created = null;

            try
            {
                var uniquePlayerName = "TestTombola_IT_create_tombola_" + Guid.NewGuid().ToString("N");
                var request = new TombolaRequest
                {
                    Name = uniquePlayerName,
                    MinPlayers = 2,
                    MaxPlayers = 5,
                    MinAwards = 1,
                    MaxAwards = 5,
                };


                var createResponse = await _client.PostAsJsonAsync("/api/tombolas", request);
                createResponse.EnsureSuccessStatusCode();


                var raw = await createResponse.Content.ReadAsStringAsync();

                created = JsonSerializer.Deserialize<TombolaResponse>(raw, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });

                created.Should().NotBeNull();
                created!.Name.Should().Be(uniquePlayerName);


                var getResponse = await _client.GetAsync($"/api/tombolas/{created.Id}");
                getResponse.EnsureSuccessStatusCode();


                var fetchedJson = await getResponse.Content.ReadAsStringAsync();

                var fetched = JsonSerializer.Deserialize<TombolaResponse>(fetchedJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });
                fetched!.Id.Should().Be(created.Id);
            }
            finally
            {
                //Cleanup
                if (created is not null)
                {
                    await _client.DeleteAsync($"/api/tombolas/{created.Id}");
                }
            }
        }

        [Fact]
        public async Task Can_Join_Tombola()
        {
            TombolaResponse? tombola = null;
            PlayerResponse? player = null;

            try
            {
                var uniquePlayerName = "TestPlayer_IT_tombola_player_" + Guid.NewGuid().ToString("N");
                var uniqueTombolaName = "TestTombola_IT_join_tombola_" + Guid.NewGuid().ToString("N");

                // 1. Create tombola
                var request = new TombolaRequest
                {
                    Name = uniqueTombolaName,
                    MinPlayers = 2,
                    MaxPlayers = 5,
                    MinAwards = 1,
                    MaxAwards = 3,
                };

                var tombolaRes = await _client.PostAsJsonAsync("/api/tombolas", request);
                tombolaRes.EnsureSuccessStatusCode();

                var tombolaJson = await tombolaRes.Content.ReadAsStringAsync();
                tombola = JsonSerializer.Deserialize<TombolaResponse>(tombolaJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });

                // 2. Create player
                var playerReq = new PlayerRequest { Name = uniquePlayerName };
                var playerRes = await _client.PostAsJsonAsync("/api/players", playerReq);
                playerRes.EnsureSuccessStatusCode();

                player = await playerRes.Content.ReadFromJsonAsync<PlayerResponse>();

                // 3. Join tombola
                var joinReq = new JoinTombolaRequest { PlayerName = player!.Name };
                var joinResponse = await _client.PostAsJsonAsync($"/api/tombolas/{tombola!.Id}/join", joinReq);
                joinResponse.EnsureSuccessStatusCode();

                // 4. Get updated tombola and assert player is in it
                var getRes = await _client.GetAsync($"/api/tombolas/{tombola.Id}");
                getRes.EnsureSuccessStatusCode();

                var json = await getRes.Content.ReadAsStringAsync();
                var updated = JsonSerializer.Deserialize<TombolaResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });

                updated!.PlayerNames.Should().Contain(player.Name);
            }
            finally
            {
                //Cleanup
                if (player is not null)
                    await _client.DeleteAsync($"/api/players/{player.Id}");

                if (tombola is not null)
                    await _client.DeleteAsync($"/api/tombolas/{tombola.Id}");
            }
        }
    }
}
