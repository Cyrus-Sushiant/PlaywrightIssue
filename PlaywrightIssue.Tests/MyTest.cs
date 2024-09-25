using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PlaywrightIssue.Tests;

[TestClass]
public partial class MyTest : PageTest
{
    [TestMethod]
    public async Task GotoCounterPage()
    {
        var builder = WebApplication.CreateBuilder(options: new()
        {
            EnvironmentName = Environments.Development
        });

        builder.WebHost.UseUrls("http://127.0.0.1:2001");

        builder.AddServerWebProjectServices();

        var app = builder.Build();

        app.ConfiureMiddlewares();

        await app.StartAsync();

        var serverAddress = new Uri(app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses.First());

        await Page.GotoAsync(new Uri(serverAddress, "counter").ToString());

        Process.Start(new ProcessStartInfo(serverAddress.ToString()) { UseShellExecute = true });

        await Task.Delay(300_000);

        await Expect(Page).ToHaveURLAsync(new Regex(@".*counter"));
    }
}
