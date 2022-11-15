namespace GitInsight.WebApi.Tests;

public class RepositoryEndpointTests : IClassFixture<CustomWebApplicationFactory> {

    private readonly HttpClient client;

    public RepositoryEndpointTests(CustomWebApplicationFactory factory){
        client = factory.CreateClient();
    }

    [Fact]
    public async Task Get()
    {
        var frequencies = await client.GetFromJsonAsync<string[]>("analysis/Mlth/BDSAProject/frequency");

        Assert.Equal("", frequencies[0]);
    }
}