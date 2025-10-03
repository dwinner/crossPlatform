using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Pages;

public partial class SettingsPage
{
   public SettingsPage(SettingsPageViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}