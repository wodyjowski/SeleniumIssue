using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.DevTools.V117.Page;

namespace SeleniumIssue;

public class SeleniumTests : IClassFixture<SeleniumTestFixture>
{
    private readonly SeleniumTestFixture fixture;

    public SeleniumTests(SeleniumTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(10000)]
    [InlineData(100000)]
    public async Task SeleniumSocketConnection_ShouldWork(int count)
    {
        var chromeOptions = new ChromeOptions();
        using var driver = new RemoteWebDriver(new Uri(fixture.GridUrl), chromeOptions);

        driver.Navigate().GoToUrl("https://google.com");

        var script = $"'{string.Join(string.Empty, Enumerable.Repeat("test", count))}'";
            
        var session = driver.GetDevToolsSession();
        await session.SendCommand(new AddScriptToEvaluateOnLoadCommandSettings()
        {
            ScriptSource = script
        });
    }
}