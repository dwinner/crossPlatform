using Foundation;

namespace MeTracker.Mac;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
   protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}