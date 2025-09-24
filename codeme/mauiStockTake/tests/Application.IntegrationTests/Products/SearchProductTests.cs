using FluentAssertions;
using MauiStockTake.Application.Products.Search;
using NUnit.Framework;

namespace MauiStockTake.Application.IntegrationTests.Products;

using static Testing;

public class SearchProductTests : TestBase
{
    [Test]
    public async Task ShouldReturnProductForValidSearch()
    {
        var query = new ProductSeachQuery { SearchTerm = "board" };

        var result = await SendAsync(query);

        result.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldNotReturnProductForInvalidSearch()
    {
        var query = new ProductSeachQuery { SearchTerm = "whydopeoplecomparefirendsandseinfeldtheyrecompletelydifferent" };

        var result = await SendAsync(query);

        result.Should().HaveCount(0);
    }
}
