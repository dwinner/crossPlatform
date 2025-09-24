using MauiStockTake.Client.Authentication;

namespace MauiStockTake.Client.Services;
public class BaseService
{
    protected HttpClient _httpClient;

    protected string _baseUrl;

    public BaseService(IHttpClientFactory httpClientFactory, ApiClientOptions options)
    {
        _httpClient = httpClientFactory.CreateClient(AuthHandler.AUTHENTICATED_CLIENT);

        _baseUrl = options.BaseUrl;
    }
}
