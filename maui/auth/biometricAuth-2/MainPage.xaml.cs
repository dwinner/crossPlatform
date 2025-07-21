using Plugin.Maui.Biometric;

namespace c5_BiometricAuth;

public partial class MainPage
{
   private readonly IBiometric _bioAuthSvc = BiometricAuthenticationService.Default;

   public MainPage()
   {
      InitializeComponent();
   }

   private async void OnCounterClicked(object sender, EventArgs e)
   {
      var result = await _bioAuthSvc.AuthenticateAsync(
         new AuthenticationRequest
         {
            Title = "Touch the fingerprint sensor",
            NegativeText = "Cancel"
         }, CancellationToken.None);

      var (title, message, cancelTxt) = result.Status == BiometricResponseStatus.Success
         ? ("Success", "System user fingerprint is recognized", "OK")
         : ("Rejected", "Couldn't authenticate", "OK");
      await DisplayAlert(title, message, cancelTxt);
   }
}