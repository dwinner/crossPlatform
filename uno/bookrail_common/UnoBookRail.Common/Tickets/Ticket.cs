namespace UnoBookRail.Common.Tickets;

public class Ticket
{
    public readonly DateTime CreationTime;
    public readonly string EndStation;
    public readonly string Price;
    public readonly string StartStation;
    public readonly string TicketID;
    public readonly PricingOptionType Type;
    public readonly string UserID;

    public Ticket(PricingOptionType type, string userID, string price, string ticketID, string startStation,
        string endStation)
    {
        Type = type;
        UserID = userID;
        Price = price;
        CreationTime = DateTime.Now;
        TicketID = ticketID;
        StartStation = startStation;
        EndStation = endStation;
    }
}