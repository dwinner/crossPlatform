using Microsoft.AspNetCore.SignalR;

namespace SignalRServer;

public class BidsHub : Hub
{
    public static bool IsAuctionRunning = true;
    public void AcceptBid(string winner)
    {
        IsAuctionRunning = false;
    }
}
