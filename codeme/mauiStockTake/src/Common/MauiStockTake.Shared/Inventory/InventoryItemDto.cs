namespace MauiStockTake.Shared.Inventory.Queries;

public class InventoryItemDto
{
    public int Id { get; set; }

    public string CountedById { get; set; }

    public string CountedByName { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string ManufacturerName { get; set; }

    public DateTime CountedAt { get; set; }

    public int Count { get; set; }
}
