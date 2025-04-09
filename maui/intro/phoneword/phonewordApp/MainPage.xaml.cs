namespace phonewordApp;

public partial class MainPage
{
   private string? _translatedNumber;

   public MainPage()
   {
      InitializeComponent();
   }

   private void OnTranslate(object? sender, EventArgs e)
   {
      var enteredNumber = phoneNumberText.Text;
      _translatedNumber = PhonewordTranslator.ToNumber(enteredNumber);
      if (!string.IsNullOrEmpty(_translatedNumber))
      {
         callButton.IsEnabled = true;
         callButton.Text = $"Call {_translatedNumber}";
      }
      else
      {
         callButton.IsEnabled = false;
         callButton.Text = "Call";
      }
   }

   private async void OnCall(object? sender, EventArgs e)
   {
      var accepted = await DisplayAlert(
         "Dial a number",
         $"Would you like to call {_translatedNumber}?",
         "Yes",
         "No",
         FlowDirection.LeftToRight).ConfigureAwait(false);
      if (!accepted)
      {
         return;
      }

      try
      {
         var phoneDialer = PhoneDialer.Default;
         if (phoneDialer.IsSupported && _translatedNumber != null)
         {
            phoneDialer.Open(_translatedNumber);
         }
      }
      catch (ArgumentNullException)
      {
         await DisplayAlert("Unable to dial", "Phone number wasn't valid", "Ok")
            .ConfigureAwait(false);
      }
      catch (Exception)
      {
         await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK")
            .ConfigureAwait(false);
      }
   }
}