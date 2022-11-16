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
        /*var httpResponse = await client.GetAsync("https://localhost:7024/analysis/Mlth/BDSAProject/frequency");
        var contentStream = await httpResponse.Content.ReadAsStreamAsync();

        using var streamReader = new StreamReader(contentStream);
        using var jsonReader = new JsonTextReader(streamReader);

        JsonSerializer serializer = new JsonSerializer();

        var deserializedOutput = serializer.Deserialize<string[]>(jsonReader);
        Assert.Equal("   s", );*/
        
        var json = await client.GetAsync("analysis/Mlth/BDSAProject/frequency");
        string jsonString = await json.Content.ReadAsStringAsync();

        var jsonStringWithoutUnnecessaryChars = jsonString.Replace("\r\n", "").Replace($" \"Frequencies\": [", "").Replace($"\"", "").Replace("{", "").Replace("}", "");

        var frequencyArray = jsonStringWithoutUnnecessaryChars.Split(",");
        for(int i = 0; i < frequencyArray.Count(); i++){
            frequencyArray[i] = frequencyArray[i].Trim();
        }

        Assert.Equal<Object>("16 15-11-2022 00:00:00", frequencyArray[0]);
    }
}