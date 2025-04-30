using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace AnalyzingActions;

public partial class SecondPage : ContentPage
{
	public SecondPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Analytics.TrackEvent("Second page opened");
    }
    protected override void OnDisappearing()
    {
        try
        {
            base.OnDisappearing();
            Analytics.TrackEvent("Second page closed");
            throw new InvalidOperationException();
        }
        catch (Exception ex)
        {
            Crashes.TrackError(ex);
        }
    }


}