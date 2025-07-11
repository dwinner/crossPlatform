namespace c4_LocalDatabaseConnection.HttpCommunication;

public static class CustomHttpMessageHandler
{
   private static readonly HttpMessageHandler PlatformHttpMessageHandler;
   public static HttpMessageHandler GetMessageHandler() => PlatformHttpMessageHandler;
}