using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c5_AuthenticationClient;

public partial class MainViewModel : ObservableObject
{
   [ObservableProperty] private string _email;

   [ObservableProperty] private string _password;

   private readonly WebService _webService = WebService.Instance;

   [RelayCommand]
   private async Task LogInAsync()
   {
      try
      {
         var tokenInfo = await _webService.Authenticate(Email, Password);
         await Shell.Current.DisplayAlert("Token", tokenInfo.AccessToken, "OK");
      }
      catch (Exception ex)
      {
         await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
      }
   }
}