using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using MusicStoreApp.Models;
using ReactiveUI;

namespace MusicStoreApp.ViewModels;

public class MusicStoreViewModel : ViewModelBase
{
   private CancellationTokenSource? _cancellationTokenSource;
   private bool _isBusy;
   private string? _searchText;
   private AlbumViewModel? _selectedAlbum;

   public MusicStoreViewModel()
   {
      this.WhenAnyValue(x => x.SearchText)
         .Throttle(TimeSpan.FromMilliseconds(400))
         .ObserveOn(RxApp.MainThreadScheduler)
         .Subscribe(DoSearch);
      BuyMusicCommand = ReactiveCommand.Create(() => SelectedAlbum);
   }

   public ReactiveCommand<Unit, AlbumViewModel?> BuyMusicCommand { get; }

   public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();

   public string? SearchText
   {
      get => _searchText;
      set => this.RaiseAndSetIfChanged(ref _searchText, value);
   }

   public bool IsBusy
   {
      get => _isBusy;
      set => this.RaiseAndSetIfChanged(ref _isBusy, value);
   }

   public AlbumViewModel? SelectedAlbum
   {
      get => _selectedAlbum;
      set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
   }

   private async void DoSearch(string? searchTerm)
   {
      IsBusy = true;
      SearchResults.Clear();
      if (_cancellationTokenSource != null)
      {
         await _cancellationTokenSource.CancelAsync();
      }

      _cancellationTokenSource = new CancellationTokenSource();
      var cancellationToken = _cancellationTokenSource.Token;
      if (!string.IsNullOrWhiteSpace(searchTerm))
      {
         var albums = await Album.SearchAsync(searchTerm);
         foreach (var album in albums)
         {
            var vm = new AlbumViewModel(album);
            SearchResults.Add(vm);
         }

         if (!cancellationToken.IsCancellationRequested)
         {
            LoadCovers(cancellationToken);
         }
      }

      IsBusy = false;
   }

   private async void LoadCovers(CancellationToken cancellationToken)
   {
      foreach (var album in SearchResults.ToList())
      {
         await album.LoadCover();
         if (cancellationToken.IsCancellationRequested)
         {
            return;
         }
      }
   }
}