using System.Diagnostics;
using MovieCatalog.ViewModels;

namespace MovieCatalog.Views;

public partial class MoviesListPage
{
   public MoviesListPage()
   {
      InitializeComponent();
   }

   private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
   {
      await Navigation.PushAsync(new MovieDetailPage());
      Debug.WriteLine(nameof(ListView_ItemTapped));
   }

   private void MenuItem_Clicked(object sender, EventArgs e)
   {
      var menuItem = (MenuItem)sender;
      var movie = (MovieViewModel)menuItem.BindingContext;
      App.MainViewModel.DeleteMovie(movie);
   }
}