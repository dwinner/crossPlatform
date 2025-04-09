using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Pages;

public partial class LoginPage
{
   public LoginPage(LoginPageViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}