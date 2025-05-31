using System.Text.Json;

namespace Gallery.Core.Services;

public sealed class MauiLocalStorage : ILocalStorage
{
   private const string FavoritePhotosKey = nameof(FavoritePhotosKey);

   public void Store(string aFilename)
   {
      var filenames = GetFiles();
      filenames.Add(aFilename);
      var favAsJson = JsonSerializer.Serialize(filenames);
      Preferences.Set(FavoritePhotosKey, favAsJson);
   }

   public List<string> GetFiles()
   {
      if (Preferences.ContainsKey(FavoritePhotosKey))
      {
         var filenames = Preferences.Get(FavoritePhotosKey, string.Empty);
         var favorites = JsonSerializer.Deserialize<List<string>>(filenames);
         return favorites ?? [];
      }

      return [];
   }
}