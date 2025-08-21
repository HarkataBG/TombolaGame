using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TombolaGame.Models.Mappers;

namespace TombolaGame.Tests.Integration
{
    public class AwardIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;


        public AwardIntegrationTests()
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
        public async Task Can_Create_Update_And_Get_Award()
        {
            AwardResponse? created = null;

            try
            {
                
                var uniqueName = "TestName_IT_Create_Award_" + Guid.NewGuid().ToString("N");
                var request = new AwardRequest { Name = uniqueName };

                var createResponse = await _client.PostAsJsonAsync("/api/awards", request);
                createResponse.EnsureSuccessStatusCode();

                // 1. Create Award
                created = await createResponse.Content.ReadFromJsonAsync<AwardResponse>();
                created.Should().NotBeNull();
                created!.Name.Should().Be(uniqueName);

                var getResponse = await _client.GetAsync($"/api/awards/{created.Id}");
                getResponse.EnsureSuccessStatusCode();

                var fetched = await getResponse.Content.ReadFromJsonAsync<AwardResponse>();
                fetched!.Id.Should().Be(created.Id);

                // 2. Update Award
                var updatedName = "Updated_Award_" + Guid.NewGuid().ToString("N");
                var updateRequest = new AwardRequest { Name = updatedName };

                var updateResponse = await _client.PutAsJsonAsync($"/api/awards/{created.Id}", updateRequest);
                updateResponse.EnsureSuccessStatusCode();

                var updatedGetResponse = await _client.GetAsync($"/api/awards/{created.Id}");
                updatedGetResponse.EnsureSuccessStatusCode();

                var updatedFetched = await updatedGetResponse.Content.ReadFromJsonAsync<AwardResponse>();
                updatedFetched.Should().NotBeNull();
                updatedFetched!.Name.Should().Be(updatedName);
            }
            finally
            {
                // 3. Cleanup
                if (created is not null)
                    await _client.DeleteAsync($"/api/awards/{created.Id}");
            }
        }
    }
}
