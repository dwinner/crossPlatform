using MauiStockTake.Application.Products.Common;
using MauiStockTake.Application.Products.GetProduct;
using MauiStockTake.Application.Products.Search;
using MauiStockTake.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MauiStockTake.WebUI.Controllers;

[Authorize]
public class ProductsController : ApiControllerBase
{
    [HttpGet("search/{searchterm}")]
    public async Task<List<ProductDto>> SearchProducts(string searchterm)
    {
        return await Mediator.Send(new ProductSeachQuery { SearchTerm = searchterm });
    }

    [HttpGet("{barcode}")]
    public async Task<ProductDto> Get(string code)
    {
        return await Mediator.Send(new GetProductByBarcodeQuery { BarCode = code });
    }
}
