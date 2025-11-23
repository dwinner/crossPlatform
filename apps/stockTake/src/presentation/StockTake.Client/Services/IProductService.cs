namespace StockTake.Client.Services;

public interface IProductService
{
   Task<List<ProductDto>> SearchProducts(string searchTerm);
}