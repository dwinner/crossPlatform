using System.Collections.ObjectModel;
using System.Data.OleDb;
using Gallery.Core;
using Gallery.Core.Models;
using Gallery.Core.Services;
using Microsoft.Search.Interop;

namespace gallery.WinUI;

internal sealed class WinUiPhotoImporter : IPhotoImporter
{
   private ISearchQueryHelper _queryHelper = null!;

   public async Task<string[]> ImportAsync()
   {
      var paths = new List<string>();
      var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync()
         .ConfigureAwait(true);
      if (status == PermissionStatus.Granted)
      {
         var sqlQuery = _queryHelper.GenerateSQLFromUserQuery(" ");
         await using OleDbConnection conn = new(_queryHelper.ConnectionString);
         conn.Open();
         await using OleDbCommand command = new(sqlQuery, conn);
         await using var wdsResults = command.ExecuteReader();
         while (wdsResults.Read())
         {
            var itemUrl = wdsResults.GetString(0);
            paths.Add(itemUrl);
         }
      }

      return paths.ToArray();
   }

   public async Task<ObservableCollection<Photo>> GetPhotosAsync(int start, int count, Quality quality = Quality.Low)
   {
      string[] patterns = [".png", ".jpeg", ".jpg"];
      string[] locations =
      {
         Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
         Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures),
         Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive", "Camera Roll"
         )
      };

      _queryHelper = new CSearchManager().GetCatalog("SystemIndex").GetQueryHelper();
      _queryHelper.QueryMaxResults = start + count;
      _queryHelper.QuerySelectColumns = "System.ItemUrl";
      _queryHelper.QueryWhereRestrictions = "AND (";
      foreach (var pattern in patterns)
      {
         _queryHelper.QueryWhereRestrictions += " Contains(System.FileExtension, '" + pattern + "') OR";
      }

      _queryHelper.QueryWhereRestrictions = _queryHelper.QueryWhereRestrictions[..^2];
      _queryHelper.QueryWhereRestrictions += ")";
      _queryHelper.QueryWhereRestrictions += " AND (";
      foreach (var location in locations)
      {
         _queryHelper.QueryWhereRestrictions += " scope='" + location + "' OR";
      }

      _queryHelper.QueryWhereRestrictions = _queryHelper.QueryWhereRestrictions[..^2];
      _queryHelper.QueryWhereRestrictions += ")";
      _queryHelper.QuerySorting = "System.DateModified DESC";

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

      foreach (var uri in result[startIndex..endIndex])
      {
         var path = new Uri(uri).AbsolutePath;
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
      _queryHelper = new CSearchManager().GetCatalog("SystemIndex").GetQueryHelper();
      _queryHelper.QuerySelectColumns = "System.ItemUrl";
      _queryHelper.QueryWhereRestrictions = "AND (";
      foreach (var filename in filenames)
      {
         _queryHelper.QueryWhereRestrictions += " Contains(System.Filename, '" + filename + "') OR";
      }

      _queryHelper.QueryWhereRestrictions = _queryHelper.QueryWhereRestrictions[..^2];
      _queryHelper.QueryWhereRestrictions += ")";

      var photos = new ObservableCollection<Photo>();
      var result = await ImportAsync().ConfigureAwait(true);
      if (result.Length == 0)
      {
         return photos;
      }

      foreach (var uri in result)
      {
         var path = new Uri(uri).AbsolutePath;
         var filename = Path.GetFileName(path);
         if (filenames.Contains(filename))
         {
            photos.Add(new Photo
            {
               Bytes = await File.ReadAllBytesAsync(path).ConfigureAwait(true),
               Filename = filename
            });
         }
      }

      return photos;
   }
}