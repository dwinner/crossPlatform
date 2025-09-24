namespace MauiStockTake.Domain.Entities;
public class Manufacturer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();
}
