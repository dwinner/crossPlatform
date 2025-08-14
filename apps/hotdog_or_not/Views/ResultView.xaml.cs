using HotdogOrNot.ViewModels;

namespace HotdogOrNot.Views;

public partial class ResultView
{
   public ResultView(ResultViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}