using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using Sharp7.Sample.Components;
using Sharp7.Sample.Services;
using System.Diagnostics.Metrics;

namespace Sharp7.Sample;

public partial class FormMain : Form
{
    public FormMain()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();
        services.AddSingleton<PLCService>();
        BWV_Main.HostPage = "wwwroot\\index.html";
        BWV_Main.Services = services.BuildServiceProvider();
        BWV_Main.RootComponents.Add<Sharp7Sample>("#app");
    }
}
