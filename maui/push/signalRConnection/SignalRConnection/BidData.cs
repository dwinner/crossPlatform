namespace SignalRConnection;

public class BidData(string bidder, decimal price)
{
   public string Bidder { get; set; } = bidder;
   public decimal Price { get; set; } = price;
}