namespace StockTake.Mobile.UI.Pages;

public partial class InputPage : ContentPage
{
	private readonly InputViewModel _viewModel;

	public InputPage(InputViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
        _viewModel.Navigation = Navigation;
		BindingContext = _viewModel;
	}
}