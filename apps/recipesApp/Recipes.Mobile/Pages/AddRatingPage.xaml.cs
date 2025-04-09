using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Pages;

public partial class AddRatingPage
{
   public AddRatingPage(AddRatingViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}