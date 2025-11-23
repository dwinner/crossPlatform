using FluentAssertions;
using MauiStockTake.Application.Inventory.Queries;
using MauiStockTake.Application.Products.Search;
using MauiStockTake.Domain.Entities;
using NUnit.Framework;

namespace MauiStockTake.Application.IntegrationTests.Inventory.Queries;

using static Testing;

public class GetInventoryTests : TestBase
{
    [Test]
    public async Task ShouldReturnAllProductCounts()
    {
        var userId = await RunAsDefaultUserAsync();

        var productQuery = new ProductSeachQuery { SearchTerm = "board" };

        var productResult = await SendAsync(productQuery);

        var product = productResult.First();

        await AddAsync(new StockCount
        {
            CountedAt = DateTime.Now,
            Count = 3,
            CountedById = userId,
            ProductId = product.Id
        });

        var query = new GetInventoryQuery();

        var result = await SendAsync(query);

        result.Inventory.Where(i => i.ProductId == product.Id).Should().HaveCount(1);
        result.Inventory.Where(i => i.ProductId == product.Id).First().Count.Should().Be(3);
    }
}
