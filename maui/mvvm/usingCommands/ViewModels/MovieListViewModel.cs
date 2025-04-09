using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MovieCatalog.Models;

namespace MovieCatalog.ViewModels;

public class MovieListViewModel : ObservableObject
{
   private MovieViewModel? _selectedMovie;

   public MovieListViewModel()
   {
      Movies = [];
      DeleteMovieCommand = new RelayCommand<MovieViewModel>(DeleteMovie);
   }

   public ICommand DeleteMovieCommand { get; }

   public MovieViewModel? SelectedMovie
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

   private void DeleteMovie(MovieViewModel? movie)
   {
      if (movie != null)
      {
         Movies.Remove(movie);
      }
   }
}