namespace MovieCatalog.Views;

public partial class MovieDetailPage
{
   public MovieDetailPage()
   {
      BindingContext = App.MainViewModel.SelectedMovie;
      InitializeComponent();
   }
}