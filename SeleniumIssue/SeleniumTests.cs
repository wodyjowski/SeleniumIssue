using System.Diagnostics;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.DevTools.V111.Page;

namespace SeleniumIssue;

public class SeleniumTests
{
    public SeleniumTests()
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = "docker",
            Arguments = "compose -f ./selenium-grid.yml up -d",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        var proc = Process.Start(startInfo);
        
        proc!.WaitForExit();
    }

    [Theory]
    [InlineData("http://localhost:4444")]
    [InlineData("http://localhost:4445")]
    public async Task TestSeleniumScriptExecution(string gridUri)
    {
        var chromeOptions = new ChromeOptions();
        var driver = new RemoteWebDriver(new Uri(gridUri), chromeOptions);

        driver.Navigate().GoToUrl("https://google.com");

        var script = $"'{string.Join(string.Empty, Enumerable.Repeat("test", 100000))}'";
        
        try
        {
            var session = driver.GetDevToolsSession();
            await session.SendCommand(new AddScriptToEvaluateOnLoadCommandSettings()
            {
                ScriptSource = script
            });
        }
        catch
        {
            driver.Dispose();
            throw;
        }
    }
}