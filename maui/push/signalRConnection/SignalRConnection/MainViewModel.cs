using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRConnection;

public partial class MainViewModel : ObservableObject
{
   private const string AuctionEndPoint = "http://localhost:5171/auction";
   [ObservableProperty] private ObservableCollection<BidData> _bids = new();

   private HubConnection _hubConnection;
   private bool _isBidAccepted;

   private bool CanAcceptBid => !_isBidAccepted;

   [RelayCommand]
   private async Task Initialize()
   {
      _hubConnection = new HubConnectionBuilder()
         .WithUrl(AuctionEndPoint)
         .Build();
      _hubConnection.On<BidData>("BidReceived", bid => { Bids.Insert(0, bid); });
      await _hubConnection.StartAsync();
   }

   [RelayCommand(CanExecute = nameof(CanAcceptBid))]
   private async Task AcceptBid(BidData bid)
   {
      await _hubConnection.InvokeCoreAsync("AcceptBid", [bid.Bidder]);
      _isBidAccepted = true;
   }
}