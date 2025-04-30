using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace AnalyzingActions;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

    private async void FirstPageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FirstPage());
    }

    private async void SecondPageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SecondPage());
    }


    private async void HelpButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Help",
    "This app will help you understand how App Center Analytics work",
    "OK");
        Analytics.TrackEvent("Help button pressed");

    }

    bool? isAnalyticsConsentSet = null;
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!isAnalyticsConsentSet.HasValue)
        {
            bool result = await DisplayAlert("Consent",
                "Do you provide your consent for analytics?", "Yes", "No");
            await Analytics.SetEnabledAsync(result);
            await Crashes.SetEnabledAsync(result);
            isAnalyticsConsentSet = result;
        }
    }

}

