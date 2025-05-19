using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Gallery.Core.Models;
using Gallery.Core.Services;

namespace Gallery.Core.ViewModels;

public partial class GalleryViewModel(IPhotoImporter photoImporter, ILocalStorage localStorage) : ViewModelBase
{
   [ObservableProperty] private ILocalStorage _localStorage = localStorage;

   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

   private int _currentStartIndex;
   private int _itemsAdded;

   [ObservableProperty] private ObservableCollection<Photo> _photos = null!;

   [RelayCommand]
   public async Task LoadMore()
   {
      _currentStartIndex += DefaultPhotoCount;
      _itemsAdded = 0;
      var collection = await photoImporter.GetPhotosAsync(_currentStartIndex, DefaultPhotoCount);
      collection.CollectionChanged += Collection_CollectionChanged;
   }

   [RelayCommand]
   public void AddFavorites(List<Photo> photos)
   {
      foreach (var photo in photos)
      {
         LocalStorage.Store(photo.Filename);
      }

      _messenger.Send<string>(Messages.FavoritesAddedMessage);
   }

   private void Collection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
   {
      var items = e.NewItems?.Cast<Photo>();
      if (items == null)
      {
         return;
      }

      foreach (var photo in items)
      {
         _itemsAdded++;
         Photos.Add(photo);
      }

      if (_itemsAdded == DefaultPhotoCount)
      {
         if (sender is ObservableCollection<Photo> collection)
         {
            collection.CollectionChanged -= Collection_CollectionChanged;
         }
      }
   }

   protected internal override async Task Initialize()
   {
      try
      {
         IsBusy = true;
         Photos = await photoImporter.GetPhotosAsync(0, DefaultPhotoCount);
         Photos.CollectionChanged += Photos_CollectionChanged;
         await Task.Delay(TimeSpan.FromSeconds(3));
      }
      finally
      {
         IsBusy = false;
      }
   }

   private void Photos_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
   {
      if (e.NewItems is { Count: > 0 })
      {
         IsBusy = false;
         Photos.CollectionChanged -= Photos_CollectionChanged;
      }
   }
}