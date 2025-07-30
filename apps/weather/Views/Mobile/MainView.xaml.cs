using Weather.ViewModels;

namespace Weather.Views.Mobile;

public partial class MainView : IMainView
{
   public MainView(MainViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;
   }

   protected override void OnNavigatedTo(NavigatedToEventArgs args)
   {
      base.OnNavigatedTo(args);
      if (BindingContext is MainViewModel viewModel)
      {
         MainThread.BeginInvokeOnMainThread(async () => { await viewModel.LoadDataAsync(); });
      }
   }
}