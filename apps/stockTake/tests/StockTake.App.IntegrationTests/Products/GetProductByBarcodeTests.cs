using FluentAssertions;
using StockTake.App.Common.Exceptions;
using StockTake.App.Products.GetProduct;

namespace StockTake.App.IntegrationTests.Products;

using static Testing;

public class GetProductByBarcodeTests : TestBase
{
    [Test]
    public async Task ShouldReturnProductWithValidBarcode()
    {
        var query = new GetProductByBarcodeQuery { BarCode = "DEF123" };

        var result = await SendAsync(query);

        result.ManufacturerName.Should().Be("Mad Lad Boards");
        result.Name.Should().Be("Mad Longboard");
    }

    [Test]
    public async Task ShouldNotReturnProductWithInvalidBarcode()
    {
        var query = new GetProductByBarcodeQuery { BarCode = "ZZZZZ" };

        await FluentActions.Invoking(() =>
        SendAsync(query)).Should().ThrowAsync<NotFoundException>();
    }
}
