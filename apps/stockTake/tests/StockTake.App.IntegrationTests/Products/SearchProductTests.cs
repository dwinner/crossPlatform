using FluentAssertions;
using StockTake.App.Products.Search;
using StockTake.Shared.Products;

namespace StockTake.App.IntegrationTests.Products;

using static Testing;

public class SearchProductTests : TestBase
{
   [Test]
   public async Task ShouldReturnProductForValidSearch()
   {
      ProductSeachQuery query = new() { SearchTerm = "board" };

      List<ProductDto> result = await SendAsync(query);

      result.Should().HaveCount(3);
   }

   [Test]
   public async Task ShouldNotReturnProductForInvalidSearch()
   {
      ProductSeachQuery query = new() { SearchTerm = "whydopeoplecomparefirendsandseinfeldtheyrecompletelydifferent" };

      List<ProductDto> result = await SendAsync(query);

      result.Should().HaveCount(0);
   }
}