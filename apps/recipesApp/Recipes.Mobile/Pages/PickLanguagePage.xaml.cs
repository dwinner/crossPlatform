using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Pages;

public partial class PickLanguagePage
{
   public PickLanguagePage(PickLanguageViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}