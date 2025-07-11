using System.Net.Security;
using Javax.Net.Ssl;
using Xamarin.Android.Net;
using Object = Java.Lang.Object;

namespace c4_LocalDatabaseConnection;

public static class CustomHttpMessageHandler
{
   private static readonly HttpMessageHandler PlatformHttpMessageHandler;
   
   static CustomHttpMessageHandler()
      => PlatformHttpMessageHandler = new CustomAndroidMessageHandler();
}

internal class CustomAndroidMessageHandler : AndroidMessageHandler
{
   public CustomAndroidMessageHandler()
      => ServerCertificateCustomValidationCallback = (_, cert, _, errors)
         => cert is { Issuer: "CN=localhost" } || errors == SslPolicyErrors.None;

   protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
      => new HostnameVerifier();

   private sealed class HostnameVerifier : Object, IHostnameVerifier
   {
      public bool Verify(string hostname, ISSLSession session)
         => HttpsURLConnection.DefaultHostnameVerifier!.Verify(hostname, session) ||
            (hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost");
   }
}