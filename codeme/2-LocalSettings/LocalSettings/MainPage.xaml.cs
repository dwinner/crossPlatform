namespace LocalSettings;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

    private bool CheckCellularConnection()
    {
        var profiles = Connectivity.ConnectionProfiles;
        return profiles.Count() == 1 &&
               profiles.Contains(ConnectionProfile.Cellular);
    }

    private bool useCellularNetwork;
    private bool hasPassword;
    protected override async void OnAppearing()
    {
        // Assuming you have declared a bool field called hasPassword
        string password = await SecureStorage.GetAsync("Password");
        if (password != null)
            hasPassword = true;

        if (CheckCellularConnection())
        {
            if (!Preferences.ContainsKey("UseCellularNetwork"))
            {
                bool result = await DisplayAlert("Warning",
                    "Do you agree on using cellular data when Wi-Fi is not available?",
                    "Yes", "No");
                Preferences.Set("UseCellularNetwork", result);
                useCellularNetwork = result;
            }
            else
                useCellularNetwork =
                     Preferences.Get("UseCellularNetwork", false);
        }
    }

    private async void OkButton_Clicked(object sender, EventArgs e)
    {
        // Perform password validation here...

        await SecureStorage.SetAsync("Password", PasswordEntry.Text);
    }
}

