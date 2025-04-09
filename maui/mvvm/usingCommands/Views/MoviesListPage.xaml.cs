using System.Diagnostics;

namespace MovieCatalog.Views;

public partial class MoviesListPage
{
   public MoviesListPage()
   {
      InitializeComponent();
   }

   private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
   {
      await Navigation.PushAsync(new MovieDetailPage()).ConfigureAwait(true);
      Debug.WriteLine(nameof(ListView_ItemTapped));
   }
}