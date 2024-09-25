using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightIssue.Components;
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

        var serverAddress = "http://127.0.0.1:2001";

        builder.WebHost.UseUrls(serverAddress);

        builder.WebHost.UseStaticWebAssets();

        builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

        var app = builder.Build();

        app.UseAntiforgery();

        app.UseStaticFiles();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        await app.StartAsync();

        await Page.GotoAsync($"{serverAddress}/counter");

        Process.Start(new ProcessStartInfo(serverAddress.ToString()) { UseShellExecute = true });

        await Task.Delay(300_000);

        await Expect(Page).ToHaveURLAsync(new Regex(@".*counter"));
    }
}
