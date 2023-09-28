using System.Diagnostics;

namespace SeleniumIssue;

public class SeleniumTestFixture : IAsyncLifetime
{
    public string GridUrl => "http://localhost:4444";
    
    public async Task InitializeAsync()
    {
        await RunDockerComposeCommand("up -d");
        await WaitForGrid(GridUrl);
    }

    public async Task DisposeAsync()
    {
        await RunDockerComposeCommand("down");
    }

    private async Task RunDockerComposeCommand(string command)
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = "docker",
            Arguments = $"compose -f ./selenium-grid.yml {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        var proc = Process.Start(startInfo);

        await proc!.WaitForExitAsync();
        if (proc.ExitCode != 0)
        {
            throw new Exception(await proc.StandardError.ReadToEndAsync());
        }
    }
    
    private async Task WaitForGrid(string gridUrl)
    {
        var client = new HttpClient();
        for (var i = 0; i < 50; i++)
        {
            try
            {
                var response = await client.GetAsync(gridUrl);
                if (response.IsSuccessStatusCode)
                    return;
            }
            catch
            {
                // ignored
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        throw new Exception("Grid not initialized");
    }
}