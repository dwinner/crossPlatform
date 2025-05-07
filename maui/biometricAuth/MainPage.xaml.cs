using System.Diagnostics;

namespace BiometricAuthentication;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
   }

   private async void LoginButton_Clicked(object sender, EventArgs e)
   {
      await Navigation.PushAsync(new LoginPage()).ConfigureAwait(true);
      Debug.WriteLine(nameof(LoginButton_Clicked));
   }

   private async void RegisterButton_Clicked(object sender, EventArgs e)
   {
      await Navigation.PushAsync(new RegisterPage()).ConfigureAwait(true);
      Debug.WriteLine(nameof(RegisterButton_Clicked));
   }
}