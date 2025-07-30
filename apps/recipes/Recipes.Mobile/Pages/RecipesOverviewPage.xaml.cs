using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Pages;

public partial class RecipesOverviewPage
{
   public RecipesOverviewPage(RecipesOverviewViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}