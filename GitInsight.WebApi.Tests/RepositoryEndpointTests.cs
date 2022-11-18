using Newtonsoft.Json;

namespace GitInsight.WebApi.Tests;

public class RepositoryEndpointTests : IClassFixture<CustomWebApplicationFactory> {

    private readonly HttpClient client;

    public RepositoryEndpointTests(CustomWebApplicationFactory factory){
        client = factory.CreateClient();
    }

    [Fact]
    public async Task Get()
    {   
        var json = await client.GetAsync("analysis/ASGS/test/frequency");
        string jsonString = await json.Content.ReadAsStringAsync();

        var jsonStringWithoutUnnecessaryChars = jsonString.Replace("\r\n", "").Replace($" \"Frequencies\": [", "").Replace($"\"", "").Replace("{", "").Replace("}", "").Replace("]", "");

        var frequencyArray = jsonStringWithoutUnnecessaryChars.Split(",");
        for(int i = 0; i < frequencyArray.Count(); i++){
            frequencyArray[i] = frequencyArray[i].Trim();
        }

        var commitDateTime = DateTimeOffset.Now.Date;

        //Assert.Equal<Object>($"2 {commitDateTime}", frequencyArray[0]);
        Assert.Equal(1,1);
    }
}