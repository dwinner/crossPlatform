namespace StockTake.Shared.Products;

public class ProductDto
{
   public int Id { get; set; }

   public string Name { get; set; } = string.Empty;

   public string ManufacturerName { get; set; } = string.Empty;

   public string ManufacturerId { get; set; } = string.Empty;
}