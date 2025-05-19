using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Gallery.Core.Models;
using Gallery.Core.Services;

namespace Gallery.Core.ViewModels;

public partial class MainViewModel(IPhotoImporter photoImporter, ILocalStorage localStorage) : ViewModelBase
{
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

   [ObservableProperty] private ObservableCollection<Photo> _favorites = null!;

   [ObservableProperty] private ObservableCollection<Photo> _recent = null!;

   protected internal override async Task Initialize()
   {
      var photos = await photoImporter.GetPhotosAsync(0, DefaultPhotoCount);
      Recent = photos;
      Favorites = await LoadFavorites();
      _messenger.Register<string>(this, async void (_, message) =>
      {
         try
         {
            if (message == Messages.FavoritesAddedMessage)
            {
               await MainThread.InvokeOnMainThreadAsync(LoadFavorites);
            }
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.Message);
         }
      });
   }

   private async Task<ObservableCollection<Photo>> LoadFavorites()
   {
      var filenames = localStorage.GetFiles();
      var favorites = await photoImporter.GetPhotosAsync(filenames);

      return favorites;
   }
}