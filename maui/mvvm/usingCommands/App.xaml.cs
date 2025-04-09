using MovieCatalog.ViewModels;

namespace MovieCatalog;

public partial class App
{
   public App()
   {
      InitializeComponent();
      MainPage = new AppShell();
      MainViewModel = new MovieListViewModel();
      MainViewModel.RefreshMoviesAsync();
   }

   public static MovieListViewModel MainViewModel { get; private set; }
}