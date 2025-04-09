namespace Recipes.Mobile.Misc;

internal static class HttpClientHelper
{
   internal static HttpClient GetPlatformHttpClient(string baseAddress)
   {
      var platform = DeviceInfo.Platform;
      if (platform != DevicePlatform.Android && platform != DevicePlatform.iOS)
      {
         return new HttpClient
         {
            BaseAddress = new Uri(baseAddress)
         };
      }

      var handler = new HttpsClientHandlerService();
      return new HttpClient(handler.GetPlatformMessageHandler())
      {
         BaseAddress = new Uri(baseAddress)
      };
   }
}