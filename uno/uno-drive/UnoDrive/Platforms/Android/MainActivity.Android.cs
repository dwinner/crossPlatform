using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Microsoft.Identity.Client;

namespace UnoDrive.Droid;

[Activity(
   MainLauncher = true,
   ConfigurationChanges = ActivityHelper.AllConfigChanges,
   WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
)]
public class MainActivity : ApplicationActivity
{
   protected override void OnCreate(Bundle? savedInstanceState)
   {
      AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);
      base.OnCreate(savedInstanceState);
   }

   protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
   {
      base.OnActivityResult(requestCode, resultCode, data);
      AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
   }
}
