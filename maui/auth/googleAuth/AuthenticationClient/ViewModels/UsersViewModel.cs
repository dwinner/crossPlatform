using System.Collections.ObjectModel;
using c5_AuthenticationClient.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c5_AuthenticationClient.ViewModels;

public partial class UsersViewModel : ObservableObject
{
   [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(DeleteUserCommand))]
   private bool _allowDelete;

   [ObservableProperty] private User _loggedInUser;

   private readonly WebService _service = WebService.Instance;

   [ObservableProperty] private ObservableCollection<User> _users;

   [RelayCommand]
   private async Task Initialize()
   {
      Users = new ObservableCollection<User>(await _service.GetUsersAsync());
      AllowDelete = await _service.CanDeleteUsersAsync();
      LoggedInUser = await _service.GetCurrentUserAsync();
   }

   [RelayCommand(CanExecute = nameof(CanDeleteUser))]
   private async Task DeleteUser(User user)
   {
      try
      {
         await _service.DeleteUserAsync(user.Email);
         Users.Remove(user);
      }
      catch (Exception ex)
      {
         await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
      }
   }

   private bool CanDeleteUser() => AllowDelete;
}