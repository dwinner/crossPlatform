using MauiStockTake.Client.Helpers;

namespace MauiStockTake.Client.Services;

public interface IProductService
{
    Task<List<ProductDto>> SearchProducts(string searchTerm);
}

public class ProductService : BaseService, IProductService
{
    private readonly ProductsClient _productClient;

    public ProductService(IHttpClientFactory clientFactory, ApiClientOptions options)
        : base(clientFactory, options)
    {
        _productClient = new ProductsClient(_baseUrl, _httpClient);
    }

    public async Task<List<ProductDto>> SearchProducts(string searchTerm)
    {
        try
        {
            var results = await _productClient.SearchProductsAsync(searchTerm);

            return results.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new List<ProductDto>();
        }
    }
}
