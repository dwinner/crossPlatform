namespace BiometricAuthentication;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void SavePasswordButton_Clicked(object sender, EventArgs e)
    {
        // Add more validation logic here...
        if (!string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await SecureStorage.SetAsync("P", PasswordEntry.Text);
            await DisplayAlert("Success", "Password saved", "OK");
            await Navigation.PopAsync();
        }

    }
}