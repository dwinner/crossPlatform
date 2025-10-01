using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App;

public partial class AppShell
{
   private readonly AppShellViewModel _viewModel;

   public AppShell(AppShellViewModel viewModel)
   {
      _viewModel = viewModel;
      InitializeComponent();
      BindingContext = _viewModel;
   }

   protected override void OnAppearing()
   {
      base.OnAppearing();
      _viewModel.LoadBoards();
   }
}