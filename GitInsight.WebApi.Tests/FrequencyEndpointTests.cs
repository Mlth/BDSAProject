using Newtonsoft.Json;
using GitInsight.Entities.DTOS;
using System.Globalization;

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

        var commitDate1 = frequencies[0].date.ToString("dd-MM-yyyy");
        var commitDate2 = frequencies[1].date.ToString("dd-MM-yyyy");

        //Assert.Equal<Object>($"2 {commitDateTime}", frequencyArray[0]);
        Assert.Equal(2, frequencies[0].frequency);
        Assert.Equal("01-10-2022", commitDate1);
        Assert.Equal(1, frequencies[1].frequency);
        Assert.Equal("02-10-2022", commitDate2);
    }

}