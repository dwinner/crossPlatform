using c5_AuthenticationClient.Model;
using c5_AuthenticationClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c5_AuthenticationClient;

public partial class MainViewModel : ObservableObject
{
   [ObservableProperty] private string _email;

   [ObservableProperty] private string _password;

   private readonly SessionService _sessionService = SessionService.Instance;
   private readonly WebService _webService = WebService.Instance;

   [RelayCommand]
   private async Task SessionLogInAsync()
   {
      if (await _sessionService.UseExistingSession())
      {
         await Shell.Current.GoToAsync(nameof(UsersPage));
      }
   }

   [RelayCommand]
   private async Task LogInAsync()
   {
      try
      {
         await _webService.Authenticate(Email, Password);
         await Shell.Current.GoToAsync(nameof(UsersPage));
      }
      catch (Exception ex)
      {
         await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
      }
   }

   [RelayCommand]
   private async Task GoogleSignInAsync()
   {
      try
      {
         await _webService.GoogleAuthAsync();
         await Shell.Current.GoToAsync(nameof(UsersPage));
      }
      catch (Exception ex) when (!(ex is TaskCanceledException))
      {
         await Shell.Current.DisplayAlert("Sign in failed", ex.Message, "OK");
      }
   }
}