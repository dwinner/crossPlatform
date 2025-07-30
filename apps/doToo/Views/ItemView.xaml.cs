using DoToo.ViewModels;

namespace DoToo.Views;

public partial class ItemView
{
   public ItemView(ItemViewModel viewModel)
   {
      InitializeComponent();
      viewModel.Navigation = Navigation;
      BindingContext = viewModel;
   }
}