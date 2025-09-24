namespace MauiStockTake.Domain.Entities;

public class StockCount
{
    public int Id { get; set; }

    public string CountedById { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }

    public DateTime CountedAt { get; set; }

    public int Count { get; set; }
}
