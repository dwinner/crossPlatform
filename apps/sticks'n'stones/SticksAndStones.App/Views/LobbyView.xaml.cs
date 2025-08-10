using SticksAndStones.ViewModels;

namespace SticksAndStones.Views;

public partial class LobbyView
{
   public LobbyView(LobbyViewModel viewModel)
   {
      BindingContext = viewModel;
      InitializeComponent();
   }
}