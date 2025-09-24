using Foundation;

namespace WidgetBoard.App;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
   protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}