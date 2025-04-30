namespace LocalSettings;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnSleep()
    {
        base.OnSleep();
        Preferences.Set("TimeOfLastUsage", DateTime.Now);
    }

    internal static DateTime timeOfLastUsage;
    protected override async void OnStart()
    {
        await CheckAppFirstRun();
        timeOfLastUsage = Preferences.Get("TimeOfLastUsage",
            DateTime.Now);
    }

    private async Task CheckAppFirstRun()
    {
        if (VersionTracking.IsFirstLaunchEver)
        {
            string password = await SecureStorage.GetAsync("password");
            if (password != null)
                SecureStorage.Remove("password");
        }
    }

}
