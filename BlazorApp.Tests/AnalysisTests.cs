using Radzen.Blazor;

namespace BlazorApp.Tests;


public class AnalysisTests
{
    private AnalysisCode analysisCode;

    public AnalysisTests() 
    {
        analysisCode = AnalysisCode.Instance;
    }

    [Fact]
    public void TextBox_Renders_ValueParameter()
    {
        using var ctx = new TestContext();
        var component = ctx.RenderComponent<Analysis>();
        var radzenComponent =  component.FindComponent<RadzenTextBox>();

        var value = "Test";
        radzenComponent.SetParametersAndRender(parameters => parameters.Add(p => p.Value, value));

        Assert.Contains(@$"value=""{value}""", component.Markup);
    }

    [Fact]
    public async void TextBox_OnChange_Changes_Repository_Value()
    {
        using var ctx = new TestContext();
        var component = ctx.RenderComponent<Analysis>();
        var radzenComponent =  component.FindComponent<RadzenTextBox>().Instance;
        
        await component.InvokeAsync(async () => 
        await radzenComponent.Change.InvokeAsync("Test"));

        Assert.Equal("Test", analysisCode.repository);
    }

    [Fact]
    public void Button_Renders_TextParameter()
    {
        using var ctx = new TestContext();
        var component = ctx.RenderComponent<Analysis>();
        var radzenCom = component.FindComponent<RadzenButton>();

        var text = "Test";
        radzenCom.SetParametersAndRender(parameters => parameters.Add(p => p.Text, text));

        Assert.Contains(@$"<span class=""rz-button-text"">{text}</span>", radzenCom.Markup);
    }
}