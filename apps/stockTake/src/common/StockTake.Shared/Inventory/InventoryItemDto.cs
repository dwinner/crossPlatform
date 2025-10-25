namespace StockTake.Shared.Inventory;

public class InventoryItemDto
{
   public int Id { get; set; }

   public string CountedById { get; set; } = string.Empty;

   public string CountedByName { get; set; } = string.Empty;

   public int ProductId { get; set; }

   public string ProductName { get; set; } = string.Empty;

   public string ManufacturerName { get; set; } = string.Empty;

   public DateTime CountedAt { get; set; }

   public int Count { get; set; }
}