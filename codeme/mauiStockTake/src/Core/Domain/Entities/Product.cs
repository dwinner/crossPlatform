namespace MauiStockTake.Domain.Entities;
public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Manufacturer Manufacturer { get; set; }

    public int ManufacturerId { get; set; }

    public string BarCode { get; set; }

    public List<StockCount> StockCounts { get; set; } = new();
}