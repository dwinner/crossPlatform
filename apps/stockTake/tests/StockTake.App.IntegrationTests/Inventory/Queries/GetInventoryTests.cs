using FluentAssertions;
using StockTake.App.Inventory.Queries;
using StockTake.App.Products.Search;
using StockTake.Domain.Entities;
using StockTake.Shared.Inventory;
using StockTake.Shared.Products;

namespace StockTake.App.IntegrationTests.Inventory.Queries;

using static Testing;

public class GetInventoryTests : TestBase
{
   [Test]
   public async Task ShouldReturnAllProductCounts()
   {
      string userId = await RunAsDefaultUserAsync();

      ProductSeachQuery productQuery = new() { SearchTerm = "board" };

      List<ProductDto> productResult = await SendAsync(productQuery);

      ProductDto product = productResult.First();

      await AddAsync(new StockCount
      {
         CountedAt = DateTime.Now, Count = 3, CountedById = userId, ProductId = product.Id
      });

      GetInventoryQuery query = new();

      InventoryVm result = await SendAsync(query);

      result.Inventory.Where(i => i.ProductId == product.Id).Should().HaveCount(1);
      result.Inventory.First(i => i.ProductId == product.Id).Count.Should().Be(3);
   }
}