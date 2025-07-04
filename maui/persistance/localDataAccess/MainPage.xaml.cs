namespace LocalDataAccess;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
      ViewModel = new CustomerViewModel();
      BindingContext = ViewModel;
   }

   private CustomerViewModel ViewModel { get; }
}