using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using MusicStoreApp.Models;
using ReactiveUI;

namespace MusicStoreApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
   public MainWindowViewModel()
   {
      ShowDialig = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
      BuyMusicCommand = ReactiveCommand.CreateFromTask(async () =>
      {
         var storeVm = new MusicStoreViewModel();
         var result = await ShowDialig.Handle(storeVm);
         if (result != null)
         {
            Albums.Add(result);
            await result.SaveToDiskAsync();
         }
      });
      RxApp.MainThreadScheduler.Schedule(LoadAlbums);
   }

   public ObservableCollection<AlbumViewModel> Albums { get; } = new();

   public ICommand BuyMusicCommand { get; }

   public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialig { get; }

   private async void LoadAlbums()
   {
      var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));
      foreach (var album in albums)
      {
         Albums.Add(album);
      }

      foreach (var album in Albums.ToList())
      {
         await album.LoadCover();
      }
   }
}