using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockTake.App.Products.GetProduct;
using StockTake.App.Products.Search;
using StockTake.Shared.Products;

namespace StockTake.WebServices.Controllers;

[Authorize]
public class ProductsController : ApiControllerBase
{
   [HttpGet("search/{searchterm}")]
   public async Task<List<ProductDto>> SearchProducts(string searchterm) =>
      await Mediator.Send(new ProductSeachQuery { SearchTerm = searchterm });

   [HttpGet("{barcode}")]
   public async Task<ProductDto> Get(string code) =>
      await Mediator.Send(new GetProductByBarcodeQuery { BarCode = code });
}