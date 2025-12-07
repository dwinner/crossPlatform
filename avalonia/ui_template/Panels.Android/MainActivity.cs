using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using ReactiveUI.Avalonia;

namespace Panels.Android;

[Activity(
   Label = "Panels.Android",
   Theme = "@style/MyTheme.NoActionBar",
   Icon = "@drawable/icon",
   MainLauncher = true,
   ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
   protected override AppBuilder CustomizeAppBuilder(AppBuilder builder) =>
      base.CustomizeAppBuilder(builder)
         .WithInterFont()
         .UseReactiveUI();
}