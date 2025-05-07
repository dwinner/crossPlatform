using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace BiometricAuthentication;

public partial class LoginPage
{
   public LoginPage()
   {
      InitializeComponent();
   }

   private static async Task<bool> IsPasswordSetAsync()
   {
      var result = await SecureStorage.GetAsync("P").ConfigureAwait(true);
      return result != null;
   }

   private static async Task<bool> IsLocalPasswordValidationPassingAsync(string password)
   {
      var localPassword = await SecureStorage.GetAsync("P").ConfigureAwait(true);
      return localPassword == password;
   }

   private async void PasswordLoginButton_Clicked(object sender, EventArgs e)
   {
      var isPasswordSet = await IsPasswordSetAsync().ConfigureAwait(true);
      if (!isPasswordSet)
      {
         await DisplayAlert("Error", "Password not set, register first", "OK").ConfigureAwait(true);
         await Navigation.PopAsync().ConfigureAwait(true);
         return;
      }

      if (!string.IsNullOrEmpty(passwordEntry.Text))
      {
         var localValidation = await IsLocalPasswordValidationPassingAsync(passwordEntry.Text)
            .ConfigureAwait(true);
         if (localValidation)
         {
            await DisplayAlert("Success", "Authenticated!", "OK").ConfigureAwait(true);
            // Do log in here...
         }
      }
   }

   private async void BiometricLoginButton_Clicked(object sender, EventArgs e)
   {
      var isPasswordSet = await IsPasswordSetAsync().ConfigureAwait(true);
      if (!isPasswordSet)
      {
         await DisplayAlert("Error", "Password not set, register first", "OK").ConfigureAwait(true);
         await Navigation.PopAsync().ConfigureAwait(true);
         return;
      }

      var biometricAuthAvailability = await CheckIfBiometricAuthIsAvailableAsync()
         .ConfigureAwait(true);
      if (!biometricAuthAvailability)
      {
         await DisplayAlert("Error", "Biometric authentication is not available.", "OK")
            .ConfigureAwait(true);
         return;
      }

      await BiometricAuthenticationAsync().ConfigureAwait(true);
   }

   private static async Task<bool> CheckIfBiometricAuthIsAvailableAsync()
   {
      var isBiometricAuthAvailable = await CrossFingerprint.Current.GetAvailabilityAsync()
         .ConfigureAwait(true);
      return isBiometricAuthAvailable == FingerprintAvailability.Available;
   }

   private async Task BiometricAuthenticationAsync()
   {
      var authRequest = new AuthenticationRequestConfiguration
         ("Biometric authentication", "Login with fingerprint or face ID");
      var result = await CrossFingerprint.Current.AuthenticateAsync(authRequest)
         .ConfigureAwait(true);
      if (result.Authenticated)
      {
         await DisplayAlert("Success", "Authenticated!", "OK").ConfigureAwait(true);
         // Do log in here...
      }
      else
      {
         await DisplayAlert("Error", $"Reason: {result.ErrorMessage}", "OK").ConfigureAwait(true);
      }
   }
}