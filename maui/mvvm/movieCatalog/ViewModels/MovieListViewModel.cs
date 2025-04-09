using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MovieCatalog.Models;

namespace MovieCatalog.ViewModels;

public class MovieListViewModel : ObservableObject
{
   private MovieViewModel _selectedMovie;

   public MovieListViewModel() => Movies = [];

   public MovieViewModel SelectedMovie
   {
      get => _selectedMovie;
      set => SetProperty(ref _selectedMovie, value);
   }

   public ObservableCollection<MovieViewModel> Movies { get; set; }

   public async Task RefreshMoviesAsync()
   {
      var moviesData = await MoviesDatabase.GetMovies().ConfigureAwait(true);
      foreach (var movie in moviesData)
      {
         Movies.Add(new MovieViewModel(movie));
      }
   }

   public void DeleteMovie(MovieViewModel movie) =>
      Movies.Remove(movie);
}