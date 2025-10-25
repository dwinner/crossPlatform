namespace StockTake.Shared.Inventory;

public class InventoryVm
{
   public DateTime InventoryAt { get; set; }

   public List<InventoryItemDto> Inventory { get; set; } = [];
}