using Android;
using Android.App;
using Android.Runtime;

[assembly: UsesPermission(Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Manifest.Permission.AccessFineLocation)]
[assembly: UsesPermission(Manifest.Permission.AccessWifiState)]
[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]

namespace MeTracker.Droid;

[Application]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership)
   : MauiApplication(handle, ownership)
{
   protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}