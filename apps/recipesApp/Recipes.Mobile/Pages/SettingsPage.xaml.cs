using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Pages;

public partial class SettingsPage
{
   public SettingsPage(SettingsViewModel settingsViewModel)
   {
      InitializeComponent();
      BindingContext = settingsViewModel;
   }
}