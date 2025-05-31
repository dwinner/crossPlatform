using Foundation;
using Gallery.Mac;

namespace gallery.Mac;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
   protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}