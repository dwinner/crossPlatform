namespace StockTake.Mobile.UI.Pages;

public partial class ReportPage : ContentPage
{
   private readonly ReportViewModel _viewModel;

   public ReportPage(ReportViewModel viewModel)
   {
      InitializeComponent();
      viewModel.Navigation = Navigation;
      _viewModel = viewModel;
      BindingContext = _viewModel;
   }

   protected override async void OnAppearing()
   {
      base.OnAppearing();

      await _viewModel.Init();
   }

   private void AboutMenuItem_Clicked(object sender, EventArgs e)
   {
      _viewModel.ShowAboutPage();
   }
}