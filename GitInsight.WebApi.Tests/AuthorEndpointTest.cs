using Newtonsoft.Json;
using GitInsight.Entities.DTOS;

namespace GitInsight.WebApi.Tests;

[TestCaseOrderer("GitInsight.WebApi.Tests.PriorityOrderer", "GitInsight.WebApi.Tests")]
public class AuthorEndpointTest : IClassFixture<CustomWebApplicationFactory> {

    private readonly HttpClient client;

    public AuthorEndpointTest(CustomWebApplicationFactory factory){
        client = factory.CreateClient();
    }

    [Fact, TestPriority(0)]
    public async Task Get()
    {
        Thread.Sleep(5000);
        var authors = await client.GetFromJsonAsync<AuthorDTO[]>("analysis/TestUser/TestRepo/author");

        var commitDateTime1 = new DateTime(2022, 10, 10);
        var commitDateTime2 = new DateTime(2022, 10, 11);

        Assert.Equal("aarv", authors[0].author);
        Assert.Equivalent(new List<FrequencyDTO>{new FrequencyDTO {date = commitDateTime2, frequency = 1}, new FrequencyDTO {date = commitDateTime1, frequency = 1}}, authors[0].frequencies);
        Assert.Equal("mlth", authors[1].author);
        Assert.Equivalent(new List<FrequencyDTO>{new FrequencyDTO {date = commitDateTime1, frequency = 1}}, authors[1].frequencies);
    }
}