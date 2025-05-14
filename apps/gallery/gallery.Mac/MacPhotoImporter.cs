using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Foundation;
using Gallery.Core;
using Gallery.Core.Models;
using Gallery.Core.Services;
using Photos;

namespace gallery.Mac;

internal sealed class MacPhotoImporter : IPhotoImporter
{
   private Dictionary<string, PHAsset> _assets = null!;

   public async Task<string[]> ImportAsync()
   {
      var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync()
         .ConfigureAwait(true);
      if (status == PermissionStatus.Granted)
      {
         _assets = PHAsset.FetchAssets(PHAssetMediaType.Image, null)
            .Select(nsObj => (PHAsset)nsObj)
            .ToDictionary(asset => asset.ValueForKey((NSString)"filename").ToString(), asset => asset);
      }

      var images = await Task.FromResult(_assets.Keys.ToArray()).ConfigureAwait(true);
      return images;
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
         AddImage(photos, path, _assets[path], quality);
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
         if (filenames.Contains(path))
         {
            AddImage(photos, path, _assets[path], quality);
         }
      }

      return photos;
   }

   private void AddImage(ObservableCollection<Photo> photos, string path, PHAsset asset, Quality quality)
   {
      var options = new PHImageRequestOptions
      {
         NetworkAccessAllowed = true,
         DeliveryMode = quality == Quality.Low
            ? PHImageRequestOptionsDeliveryMode.FastFormat
            : PHImageRequestOptionsDeliveryMode.HighQualityFormat
      };

      PHImageManager.DefaultManager.RequestImageForAsset(asset, PHImageManager.MaximumSize,
         PHImageContentMode.AspectFill, options, (image, _) =>
         {
            using var imageData = image.AsPNG();
            Debug.Assert(imageData != null, $"{nameof(imageData)} != null");

            var bytes = new byte[imageData.Length];
            Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
            photos.Add(new Photo
            {
               Bytes = bytes,
               Filename = Path.GetFileName(path)
            });
         });
   }
}