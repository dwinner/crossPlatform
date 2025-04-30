namespace LocalDataAccess;

public partial class MainPage : ContentPage
{
	private CustomerViewModel ViewModel { get; set; }

	public MainPage()
	{
		InitializeComponent();
        ViewModel = new CustomerViewModel();
        BindingContext = ViewModel;
    }
}

