namespace BlazorApp.Tests;

public class AnalysisTests
{
    [Fact]
    public void TestComponent()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<Analysis>();
        cut.MarkupMatches("<p>Hello from TestComponent</p>");
    }
}