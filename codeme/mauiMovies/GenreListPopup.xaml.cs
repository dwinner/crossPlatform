using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace MauiMovies;

public partial class GenreListPopup : Popup
{
    public ObservableCollection<UserGenre> Genres { get; set; }

    public GenreListPopup(List<UserGenre> Genres)
    {
        BindingContext = this;
        this.Genres = new ObservableCollection<UserGenre>(Genres);

        ResultWhenUserTapsOutsideOfPopup = _selectionHasChanged;

        InitializeComponent();
    }

    private bool _selectionHasChanged = false;

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

    private void Button_Clicked(object sender, EventArgs e) => Close(_selectionHasChanged);

}
