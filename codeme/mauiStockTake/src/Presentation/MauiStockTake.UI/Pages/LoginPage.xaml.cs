namespace MauiStockTake.UI.Pages;

public partial class LoginPage : ContentPage
{
	private readonly IAuthService _authService;

	public LoginPage(IAuthService authService)
	{
		InitializeComponent();
		_authService = authService;
	}

	private async void LoginButton_Clicked(object sender, EventArgs e)
	{
		LoginButton.IsEnabled = false;
		LoggingIn.IsVisible = true;

		var loggedIn = await _authService.LoginAsync();
        
		LoggingIn.IsVisible = false;

        if (!loggedIn)
		{
			await App.Current.MainPage.DisplayAlert("Error", "Something went wrong logging you in. Please try again.", "OK");
            LoginButton.IsEnabled = true;
            LoggingIn.IsVisible = false;
        }
		else
		{
			await Navigation.PopModalAsync();
		}
    }
}