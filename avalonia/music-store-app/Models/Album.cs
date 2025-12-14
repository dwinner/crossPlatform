using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace MusicStoreApp.Models;

public class Album
{
   private static readonly iTunesSearchManager SearchManager = new();
   private static readonly HttpClient HttpClient = new();

   public Album(string artist, string title, string coverUrl)
   {
      Artist = artist;
      Title = title;
      CoverUrl = coverUrl;
   }

   private string CachePath => $"./Cache/{Artist} - {Title}";

   public string Artist { get; set; }

   public string Title { get; set; }

   public string CoverUrl { get; set; }

   public static async Task<IEnumerable<Album>> SearchAsync(string searchTerm)
   {
      var query = await SearchManager.GetAlbumsAsync(searchTerm)
         .ConfigureAwait(false);

      return query.Albums.Select(x =>
         new Album(x.ArtistName, x.CollectionName,
            x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
   }

   public async Task<Stream> LoadCoverBitmapAsync()
   {
      var cachedPath = $"{CachePath}.bmp";
      if (File.Exists(cachedPath))
      {
         return File.OpenRead(cachedPath);
      }

      var data = await HttpClient.GetByteArrayAsync(CoverUrl);
      return new MemoryStream(data);
   }

   public async Task SaveAsync()
   {
      if (!Directory.Exists("./Cache"))
      {
         Directory.CreateDirectory("./Cache");
      }

      using (var fs = File.OpenWrite(CachePath))
      {
         await SaveToStreamAsync(this, fs);
      }
   }

   public Stream SaveCoverBitmapStream() => File.OpenWrite(CachePath + ".bmp");

   private static async Task SaveToStreamAsync(Album data, Stream stream)
   {
      await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
   }

   public static async Task<Album> LoadFromStream(Stream stream) =>
      (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;

   public static async Task<IEnumerable<Album>> LoadCachedAsync()
   {
      if (!Directory.Exists("./Cache"))
      {
         Directory.CreateDirectory("./Cache");
      }

      var results = new List<Album>();

      foreach (var file in Directory.EnumerateFiles("./Cache"))
      {
         if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension))
         {
            continue;
         }

         await using var fs = File.OpenRead(file);
         results.Add(await LoadFromStream(fs).ConfigureAwait(false));
      }

      return results;
   }
}