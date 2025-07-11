namespace c4_LocalDatabaseConnection.HttpCommunication;

public static class WebApiHttpClient
{
   public static readonly HttpClient Instance = new(CustomHttpMessageHandler.GetMessageHandler())
   {
      Timeout = new TimeSpan(0, 0, 20)
   };

   static WebApiHttpClient()
   {
#if ANDROID
      Instance.BaseAddress = new Uri("https://10.0.2.2:7197/api/");
#else
      Instance.BaseAddress = new Uri("https://localhost:7197/api/");
#endif
   }
}