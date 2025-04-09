using DoToo.ViewModels;

namespace DoToo.Views;

public partial class MainView
{
   public MainView(MainViewModel viewModel)
   {
      InitializeComponent();
      viewModel.Navigation = Navigation;
      BindingContext = viewModel;
      itemsListView.ItemSelected += (_, _) => itemsListView.SelectedItem = null;
   }
}