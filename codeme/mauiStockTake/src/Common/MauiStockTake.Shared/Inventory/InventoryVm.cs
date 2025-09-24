namespace MauiStockTake.Shared.Inventory.Queries;
public class InventoryVm
{
    public DateTime InventoryAt { get; set; }

    public List<InventoryItemDto> Inventory { get; set; } = new();
}
