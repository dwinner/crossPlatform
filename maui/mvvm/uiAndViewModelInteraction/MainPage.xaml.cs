using CommunityToolkit.Mvvm.Messaging;

namespace c2_DecoupleViewAndViewModel;

public partial class MainPage
{
   public MainPage(MyViewModel viewModel, IMessenger messenger)
   {
      InitializeComponent();

      BindingContext = viewModel;
      messenger.Register<Customer>(this,
         (_, addedCustomer) => { customersCollectionView.ScrollTo(addedCustomer); });
   }
}