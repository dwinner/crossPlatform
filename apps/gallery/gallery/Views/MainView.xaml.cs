using Gallery.Core.ViewModels;

namespace Gallery.Core.Views;

public partial class MainView
{
   public MainView(MainViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
      MainThread.InvokeOnMainThreadAsync(viewModel.Initialize);
   }
}