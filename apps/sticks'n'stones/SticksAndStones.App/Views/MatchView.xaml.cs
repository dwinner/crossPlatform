using SticksAndStones.ViewModels;

namespace SticksAndStones.Views;

public partial class MatchView
{
   public MatchView(MatchViewModel viewModel)
   {
      BindingContext = viewModel;
      InitializeComponent();
   }
}