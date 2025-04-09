using Foundation;

namespace MvvmDemo;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
   protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}