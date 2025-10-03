using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Pages;

public partial class FixedBoardPage
{
   public FixedBoardPage(FixedBoardPageViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}