using Microsoft.AppCenter.Analytics;

namespace AnalyzingActions;

public partial class FirstPage : ContentPage
{
	public FirstPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Analytics.TrackEvent("First Page opened");
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Analytics.TrackEvent("First Page closed");
    }
}