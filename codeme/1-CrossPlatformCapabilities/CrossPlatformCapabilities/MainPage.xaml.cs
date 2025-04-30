using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;

namespace CrossPlatformCapabilities;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();

        WeakReferenceMessenger.Default.Register<ConnectionChangedMessage>(this, async (sender, message) =>
        {
            await ShowConnectionSnackbarAsync(message.Value);
        });

        WeakReferenceMessenger.Default.Register<BatteryStatusChangedMessage>(
            this, (sender, message) =>
            {
                ManageBatteryLevelChanged();
            });
    }

	private async Task ShowConnectionSnackbarAsync(bool value)
	{
        var options = new SnackbarOptions
        {
            BackgroundColor = Colors.PaleVioletRed,
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
        };

        string message;
        switch(value)
        {
            case true:
                message = "Internet connection available";
                break;
            default:
                message = "Internet connection lost";
                break;
        }

        await this.DisplaySnackbar(message, visualOptions: options);
    }

    // Omitting Visual Studio auto-generated code
    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }


    private void ManageBatteryLevelChanged()
    {
        FileHelper.WriteData("test data");
    }
}

