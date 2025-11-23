using FluentAssertions;
using MauiStockTake.Application.Common.Exceptions;
using MauiStockTake.Application.Products.GetProduct;
using NUnit.Framework;

namespace MauiStockTake.Application.IntegrationTests.Products;

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
