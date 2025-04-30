using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace AnalyzingActions;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage());
	}

    protected override void OnStart()
    {
        base.OnStart();
        AppCenter.Start("android=YOUR-APP-SECRET-GOES-HERE;",
                  typeof(Analytics), typeof(Crashes));
    }
}
