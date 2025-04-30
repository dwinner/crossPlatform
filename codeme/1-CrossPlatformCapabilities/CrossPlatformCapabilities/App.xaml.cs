using CommunityToolkit.Mvvm.Messaging;

namespace CrossPlatformCapabilities;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        Battery.EnergySaverStatusChanged += Battery_EnergySaverStatusChanged;
        
        //MainPage = new AppShell();
        MainPage=new BrowserEmailSample();
	}

    private void Connectivity_ConnectivityChanged(object sender, 
        ConnectivityChangedEventArgs e)
    {
        WeakReferenceMessenger.Default.
            Send(new ConnectionChangedMessage(
                e.NetworkAccess == NetworkAccess.Internet));
    }

    protected override void OnStart()
    {
        base.OnStart();
        WeakReferenceMessenger.Default.
            Send(new ConnectionChangedMessage(
                Connectivity.NetworkAccess == NetworkAccess.Internet));
    }

    private void Battery_EnergySaverStatusChanged(object sender,
        EnergySaverStatusChangedEventArgs e)
    {
        // Remove the ChargeLevel check if your app
        // implements background services
        WeakReferenceMessenger.Default.
            Send(new BatteryStatusChangedMessage(
                e.EnergySaverStatus == EnergySaverStatus.On
                && Battery.ChargeLevel <= 0.2));
            
    }
}
