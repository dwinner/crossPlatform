using Android.App;
using Android.Content;
using Android.Content.PM;

namespace MauiStockTake.UI.Platforms.Android;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "auth.com.mildredsurf.stocktake",
    DataHost = "callback")]
public class WebCallbackActivity : WebAuthenticatorCallbackActivity
{

}
