using SticksAndStones.ViewModels;

namespace SticksAndStones.Views;

public partial class ConnectView
{
   public ConnectView(ConnectViewModel viewModel)
   {
      BindingContext = viewModel;
      InitializeComponent();
   }
}