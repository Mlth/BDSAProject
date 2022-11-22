using Radzen.Blazor;

namespace BlazorApp.Tests;


public class AnalysisTests
{
    [Fact]
    public void TextBox_Renders_ValueParameter()
    {
        using var ctx = new TestContext();

        var component = ctx.RenderComponent<Analysis>();

        var radzenCom =  component.FindComponent<RadzenTextBox>();

        var value = "Test";

        radzenCom.SetParametersAndRender(parameters => parameters.Add(p => p.Value, value));

        Assert.Contains(@$"value=""{value}""", component.Markup);
    }
}