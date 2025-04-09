using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Pages;

public partial class RecipeDetailPage
{
   public RecipeDetailPage(RecipeDetailViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}