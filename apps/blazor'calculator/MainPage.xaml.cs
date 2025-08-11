using Calculator.ViewModels;

namespace Calculator;

public partial class MainPage
{
   public MainPage(MainPageViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }
}