using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using MoviesApp.Models;

namespace MoviesApp;

public partial class GenreListPopup
{
   private bool _selectionHasChanged;

   public GenreListPopup(List<UserGenre> Genres)
   {
      BindingContext = this;
      this.Genres = new ObservableCollection<UserGenre>(Genres);
      //ResultWhenUserTapsOutsideOfPopup = _selectionHasChanged;
      InitializeComponent();
   }

   public ObservableCollection<UserGenre> Genres { get; set; }

   private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      _selectionHasChanged = true;
      var selectedItems = e.CurrentSelection;
      foreach (var genre in Genres)
      {
         if (selectedItems.Contains(genre))
         {
            genre.Selected = true;
         }
         else
         {
            genre.Selected = false;
         }
      }
   }

   private async void Button_Clicked(object sender, EventArgs e) => await CloseAsync( /*_selectionHasChanged*/);
}