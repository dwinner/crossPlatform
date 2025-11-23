using StockTake.Client.Authentication;

namespace StockTake.Client.Services;

public class BaseService
{
   protected readonly string BaseUrl;
   protected readonly HttpClient HttpClient;

   public BaseService(IHttpClientFactory httpClientFactory, ApiClientOptions options)
   {
      HttpClient = httpClientFactory.CreateClient(AuthHandler.AUTHENTICATED_CLIENT);
      BaseUrl = options.BaseUrl;
   }
}