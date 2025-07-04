using CommunityToolkit.Mvvm.Messaging;

namespace CrossPlatformCapabilities;

public partial class App
{
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

   public App()
   {
      InitializeComponent();
      Connectivity.ConnectivityChanged += OnConnectivityChanged;
      Battery.EnergySaverStatusChanged += OnEnergySaverStatusChanged;

      //MainPage = new AppShell();
      MainPage = new FilePickerPage();
   }

   private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
   {
      _messenger.Send(
         new ConnectionChangedMessage(e.NetworkAccess == NetworkAccess.Internet)
      );
   }

   protected override void OnStart()
   {
      base.OnStart();
      _messenger.Send(
         new ConnectionChangedMessage(Connectivity.NetworkAccess == NetworkAccess.Internet)
      );
   }

   private void OnEnergySaverStatusChanged(object sender, EnergySaverStatusChangedEventArgs e)
   {
      // Remove the ChargeLevel check if your app
      // implements background services
      _messenger.Send(
         new BatteryStatusChangedMessage(e.EnergySaverStatus == EnergySaverStatus.On
                                         && Battery.ChargeLevel <= 0.2)
      );
   }
}