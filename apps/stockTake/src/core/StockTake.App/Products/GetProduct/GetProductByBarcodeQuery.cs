using MediatR;
using StockTake.Shared.Products;

namespace StockTake.App.Products.GetProduct;

public class GetProductByBarcodeQuery : IRequest<ProductDto>
{
    public string BarCode { get; set; }
}