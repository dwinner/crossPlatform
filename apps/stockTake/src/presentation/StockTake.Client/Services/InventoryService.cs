using System.Diagnostics;
using MauiStockTake.Client.Helpers;

namespace StockTake.Client.Services;

public class InventoryService : BaseService, IInventoryService
{
   private readonly InventoryClient _inventoryClient;

   public InventoryService(IHttpClientFactory httpClientFactory, ApiClientOptions options) :
      base(httpClientFactory, options) => _inventoryClient = new InventoryClient(BaseUrl, HttpClient);

   public async Task<bool> AddStockCount(ProductDto product, int count)
   {
      var command = new StockCountDto
      {
         ProductId = product.Id,
         ProductCount = count
      };

      try
      {
         await _inventoryClient.AddStockCountAsync(command);
         return true;
      }
      catch (Exception ex)
      {
         Debug.WriteLine(ex.Message);
         return false;
      }
   }

   public async Task<List<InventoryItemDto>> GetInventory()
   {
      var inventory = await _inventoryClient.GetInventoryAsync();

      var stockList = new List<InventoryItemDto>();

      foreach (var item in inventory.Inventory)
      {
         stockList.Add(new InventoryItemDto
         {
            CountedAt = item.CountedAt.Date,
            CountedByName = item.CountedByName,
            ProductName = item.ProductName,
            ManufacturerName = item.ManufacturerName,
            Count = item.Count
         });
      }

      return stockList;
   }
}