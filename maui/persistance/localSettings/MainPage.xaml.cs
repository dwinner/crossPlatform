namespace LocalSettings;

public partial class MainPage
{
   private bool _hasPassword;

   private bool _useCellularNetwork;

   public MainPage()
   {
      InitializeComponent();
   }

   private static bool CheckCellularConnection()
   {
      var profiles = Connectivity.ConnectionProfiles;
      var conVariants = profiles as ConnectionProfile[] ?? profiles.ToArray();
      return conVariants.Length == 1 && conVariants.Contains(ConnectionProfile.Cellular);
   }

   protected override async void OnAppearing()
   {
      // Assuming you have declared a bool field called hasPassword
      var password = await SecureStorage.GetAsync("Password");
      if (password != null)
      {
         _hasPassword = true;
      }

      if (CheckCellularConnection())
      {
         if (!Preferences.ContainsKey("UseCellularNetwork"))
         {
            var result = await DisplayAlert("Warning",
               "Do you agree on using cellular data when Wi-Fi is not available?",
               "Yes", "No");
            Preferences.Set("UseCellularNetwork", result);
            _useCellularNetwork = result;
         }
         else
         {
            _useCellularNetwork = Preferences.Get("UseCellularNetwork", false);
         }
      }
   }

   private async void OkButton_Clicked(object sender, EventArgs e)
   {
      // Perform password validation here...

      await SecureStorage.SetAsync("Password", passwordEntry.Text);
   }
}