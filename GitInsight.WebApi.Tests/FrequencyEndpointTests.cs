using Newtonsoft.Json;
using GitInsight.Entities.DTOS;

namespace GitInsight.WebApi.Tests;

[TestCaseOrderer("GitInsight.WebApi.Tests.PriorityOrderer", "GitInsight.WebApi.Tests")]
public class FrequencyEndpointTest : IClassFixture<CustomWebApplicationFactory> {

    private readonly HttpClient client;

    public FrequencyEndpointTest(CustomWebApplicationFactory factory){
        client = factory.CreateClient();
    }

    [Fact, TestPriority(0)]
    public async Task Get()
    {
        var frequencies = await client.GetFromJsonAsync<FrequencyDTO[]>("analysis/TestUser/TestRepo/frequency");

        var commitDateTime1 = new DateTime(2022, 10, 10);
        var commitDateTime2 = new DateTime(2022, 10, 11);

        //Assert.Equal<Object>($"2 {commitDateTime}", frequencyArray[0]);
        Assert.Equal(2, frequencies[0].frequency);
        Assert.Equal("10-10-2022", frequencies[0].date.ToShortDateString());
        Assert.Equal(1, frequencies[1].frequency);
        Assert.Equal("11-10-2022", frequencies[1].date.ToShortDateString());
    }

}