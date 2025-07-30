using CommunityToolkit.Mvvm.Input;
using Recipes.Client.Core.Navigation;

namespace Recipes.Client.Core.ViewModels;

/*
 * APPLY: No functions are implemented for login/logout
 */

public class LoginPageViewModel
{
   private readonly INavigationService _navigationService;

   public LoginPageViewModel(INavigationService navigation) => _navigationService = navigation;

   //LoginCommand = new RelayCommand(() => navigationService.LoadApp());
   public RelayCommand? LoginCommand { get; }
}