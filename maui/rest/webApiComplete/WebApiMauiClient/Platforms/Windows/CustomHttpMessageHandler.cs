namespace c4_LocalDatabaseConnection;

public static class CustomHttpMessageHandler
{
   private static readonly HttpMessageHandler PlatformHttpMessageHandler;

   static CustomHttpMessageHandler()
      => PlatformHttpMessageHandler = new HttpClientHandler();
}