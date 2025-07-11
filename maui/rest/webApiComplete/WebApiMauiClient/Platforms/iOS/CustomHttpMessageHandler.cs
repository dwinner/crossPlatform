using System.Net.Security;

namespace c4_LocalDatabaseConnection;

public static class CustomHttpMessageHandler
{
   private static readonly HttpMessageHandler PlatformHttpMessageHandler;

   static CustomHttpMessageHandler()
   {
      NSUrlSessionHandler nSUrlSessionHandler = new();
      nSUrlSessionHandler.ServerCertificateCustomValidationCallback += (_, cert, _, errors)
         => cert is { Issuer: "CN=localhost" } || errors == SslPolicyErrors.None;
      nSUrlSessionHandler.TrustOverrideForUrl = (sender, url, trust) => { return true; };
      PlatformHttpMessageHandler = nSUrlSessionHandler;
   }
}