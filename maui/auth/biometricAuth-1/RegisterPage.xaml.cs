namespace BiometricAuthentication;

public partial class RegisterPage
{
   public RegisterPage()
   {
      InitializeComponent();
   }

   private async void SavePasswordButton_Clicked(object sender, EventArgs e)
   {
      // Add more validation logic here...
      if (!string.IsNullOrEmpty(passwordEntry.Text))
      {
         await SecureStorage.SetAsync("P", passwordEntry.Text).ConfigureAwait(true);
         await DisplayAlert("Success", "Password saved", "OK").ConfigureAwait(true);
         await Navigation.PopAsync().ConfigureAwait(true);
      }
   }
}