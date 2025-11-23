namespace StockTake.Client.Services;

public interface IInventoryService
{
   Task<bool> AddStockCount(ProductDto prodcut, int count);

   Task<List<InventoryItemDto>> GetInventory();
}