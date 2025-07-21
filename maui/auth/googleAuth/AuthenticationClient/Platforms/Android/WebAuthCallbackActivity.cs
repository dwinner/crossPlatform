using Android.App;
using Android.Content;
using Android.Content.PM;

namespace c5_AuthenticationClient.Platforms;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
   Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
   DataScheme = CALLBACK_SCHEME)]
public class WebAuthCallbackActivity : WebAuthenticatorCallbackActivity
{
   private const string CALLBACK_SCHEME = "myapp";
}