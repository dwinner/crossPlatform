using MediatR;
using StockTake.Shared.Products;

namespace StockTake.App.Products.Search;

public class ProductSeachQuery : IRequest<List<ProductDto>>
{
   public string SearchTerm { get; set; }
}