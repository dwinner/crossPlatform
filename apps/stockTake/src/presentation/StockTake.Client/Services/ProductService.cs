using System.Diagnostics;
using MauiStockTake.Client.Helpers;

namespace StockTake.Client.Services;

public class ProductService : BaseService, IProductService
{
   private readonly ProductsClient _productClient;

   public ProductService(IHttpClientFactory clientFactory, ApiClientOptions options)
      : base(clientFactory, options) =>
      _productClient = new ProductsClient(BaseUrl, HttpClient);

   public async Task<List<ProductDto>> SearchProducts(string searchTerm)
   {
      try
      {
         var results = await _productClient.SearchProductsAsync(searchTerm);
         return results.ToList();
      }
      catch (Exception ex)
      {
         Debug.WriteLine(ex);
         return new List<ProductDto>();
      }
   }
}