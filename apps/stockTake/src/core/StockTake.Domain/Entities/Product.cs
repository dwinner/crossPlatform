namespace StockTake.Domain.Entities;

public class Product
{
   public int Id { get; set; }

   public string Name { get; set; } = string.Empty;

   public Manufacturer? Manufacturer { get; set; }

   public int ManufacturerId { get; set; }

   public string BarCode { get; set; } = string.Empty;

   public List<StockCount> StockCounts { get; set; } = new();
}