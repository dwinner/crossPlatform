namespace LocalSettings;

public partial class App
{
   private const string TimeOfLastUsagePrefKey = "TimeOfLastUsage";
   private const string PassPrefKey = "password";

   internal static DateTime TimeOfLastUsage;

   public App()
   {
      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState activationState) => new(new AppShell());

   protected override void OnSleep()
   {
      base.OnSleep();
      Preferences.Set(TimeOfLastUsagePrefKey, DateTime.Now);
   }

   protected override async void OnStart()
   {
      await CheckAppFirstRun();
      TimeOfLastUsage = Preferences.Get(TimeOfLastUsagePrefKey, DateTime.Now);
   }

   private static async Task CheckAppFirstRun()
   {
      if (!VersionTracking.IsFirstLaunchEver)
      {
         return;
      }

      var password = await SecureStorage.GetAsync(PassPrefKey);
      if (password != null)
      {
         SecureStorage.Remove(PassPrefKey);
      }
   }
}