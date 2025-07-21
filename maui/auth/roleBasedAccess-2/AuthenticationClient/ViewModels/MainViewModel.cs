using c5_AuthenticationClient.Model;
using c5_AuthenticationClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c5_AuthenticationClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
   [ObservableProperty] private string _email;

   [ObservableProperty] private string _password;

   [RelayCommand]
   private async Task LogInAsync()
   {
      try
      {
         _ = await WebService.Instance.Authenticate(Email, Password);
         await Shell.Current.GoToAsync(nameof(UsersPage));
      }
      catch (Exception ex)
      {
         await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
      }
   }
}