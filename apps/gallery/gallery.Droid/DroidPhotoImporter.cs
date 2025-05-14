using System.Collections.ObjectModel;
using System.Diagnostics;
using Android.Provider;
using Gallery.Core;
using Gallery.Core.Models;
using Gallery.Core.Services;

namespace Gallery.Droid;

internal sealed class DroidPhotoImporter : IPhotoImporter
{
   public async Task<string[]> ImportAsync()
   {
      var paths = new List<string>();
      var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync()
         .ConfigureAwait(true);
      if (status == PermissionStatus.Granted)
      {
         var imageUri = MediaStore.Images.Media.ExternalContentUri;
         var projection = new[] { MediaStore.IMediaColumns.Data };
         const string orderBy = MediaStore.Images.IImageColumns.DateTaken;

         Debug.Assert(imageUri != null, $"{nameof(imageUri)} != null");

         var cursor = Platform.CurrentActivity?.ContentResolver?.Query(imageUri, projection, null, null, orderBy);
         Debug.Assert(cursor != null, $"{nameof(cursor)} != null");

         while (cursor.MoveToNext())
         {
            var path = cursor.GetString(
               cursor.GetColumnIndex(MediaStore.IMediaColumns.Data)
            );

            if (path != null)
            {
               paths.Add(path);
            }
         }
      }

      return paths.ToArray();
   }

   public async Task<ObservableCollection<Photo>> GetPhotosAsync(int start, int count, Quality quality = Quality.Low)
   {
      var photos = new ObservableCollection<Photo>();
      var result = await ImportAsync().ConfigureAwait(true);
      if (result.Length == 0)
      {
         return photos;
      }

      Index startIndex = start;
      Index endIndex = start + count;

      if (endIndex.Value >= result.Length)
      {
         endIndex = result.Length;
      }

      if (startIndex.Value > endIndex.Value)
      {
         return photos;
      }

      foreach (var path in result[startIndex..endIndex])
      {
         photos.Add(new Photo
         {
            Bytes = await File.ReadAllBytesAsync(path).ConfigureAwait(true),
            Filename = Path.GetFileName(path)
         });
      }

      return photos;
   }

   public async Task<ObservableCollection<Photo>> GetPhotosAsync(List<string> filenames, Quality quality = Quality.Low)
   {
      var photos = new ObservableCollection<Photo>();
      var result = await ImportAsync().ConfigureAwait(true);
      if (result.Length == 0)
      {
         return photos;
      }

      foreach (var path in result)
      {
         var filename = Path.GetFileName(path);
         if (!filenames.Contains(filename))
         {
            continue;
         }

         photos.Add(new Photo
         {
            Bytes = await File.ReadAllBytesAsync(path).ConfigureAwait(true),
            Filename = filename
         });
      }

      return photos;
   }
}