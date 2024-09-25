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
        await using var server = new AppTestServer();
        await server.Build().Start();

        await Page.GotoAsync(new Uri(server.GetServerAddress(), "counter").ToString());

        Process.Start(new ProcessStartInfo(server.GetServerAddress().ToString()) { UseShellExecute = true });

        await Task.Delay(300_000);

        await Expect(Page).ToHaveURLAsync(new Regex(@".*counter"));
    }
}
