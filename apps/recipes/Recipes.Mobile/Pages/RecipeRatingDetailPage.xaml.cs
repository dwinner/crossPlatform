using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Pages;

public partial class RecipeRatingDetailPage
{
   public RecipeRatingDetailPage(RecipeRatingsDetailViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}