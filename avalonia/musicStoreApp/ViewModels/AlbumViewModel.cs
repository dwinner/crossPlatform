using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using MusicStoreApp.Models;
using ReactiveUI;

namespace MusicStoreApp.ViewModels;

public class AlbumViewModel(Album album) : ViewModelBase
{
   private Bitmap? _cover;

   public string Artist => album.Artist;

   public string Title => album.Title;

   public Bitmap? Cover
   {
      get => _cover;
      private set => this.RaiseAndSetIfChanged(ref _cover, value);
   }

   public async Task LoadCover()
   {
      await using var imageStream = await album.LoadCoverBitmapAsync();
      Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
   }

   public async Task SaveToDiskAsync()
   {
      await album.SaveAsync();
      if (Cover != null)
      {
         var bitmap = Cover;
         await Task.Run(() =>
         {
            using (var fs = album.SaveCoverBitmapStream())
            {
               bitmap.Save(fs);
            }
         });
      }
   }
}