using LiteDB;

namespace UnoDrive.Data;

public sealed class DataStore : IDataStore
{
   private const string DbName = "UnoDriveData.db";
   private readonly string _databaseFile;

   public DataStore()
   {
#if HAS_UNO_SKIA_WPF
			var applicationFolder = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, "UnoDrive");
			databaseFile = Path.Combine(applicationFolder, "UnoDriveData.db");
#else
      _databaseFile = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
#endif
   }

   public void SaveUserInfo(UserInfo userInfo)
   {
      using var driveDb = new LiteDatabase(_databaseFile);
      var users = driveDb.GetCollection<UserInfo>();
      var findUserInfo = users.FindById(userInfo.Id);

      if (findUserInfo != null)
      {
         findUserInfo.Name = userInfo.Name;
         findUserInfo.Email = userInfo.Email;
         users.Update(findUserInfo);
      }
      else
      {
         users.Insert(userInfo);
      }
   }

   public UserInfo GetUserInfoById(string userId)
   {
      using var driveDb = new LiteDatabase(_databaseFile);
      var users = driveDb.GetCollection<UserInfo>();
      return users.FindById(userId);
   }

   public void SaveRootId(string rootId)
   {
      using var liteDb = new LiteDatabase(_databaseFile);
      var settings = liteDb.GetCollection<Setting>();
      var findRootIdSetting = settings.FindById("RootId");
      if (findRootIdSetting != null)
      {
         findRootIdSetting.Value = rootId;
         settings.Update(findRootIdSetting);
      }
      else
      {
         var newSetting = new Setting { Id = "RootId", Value = rootId };
         settings.Insert(newSetting);
      }
   }

   public string GetRootId()
   {
      using var liteDb = new LiteDatabase(_databaseFile);
      var settings = liteDb.GetCollection<Setting>();
      var rootId = settings.FindById("RootId");

      return rootId != null ? rootId.Value : string.Empty;
   }

   public IEnumerable<OneDriveItem> GetCachedFiles(string pathId)
   {
      if (string.IsNullOrEmpty(pathId))
      {
         return Array.Empty<OneDriveItem>();
      }

      using var liteDb = new LiteDatabase(_databaseFile);
      var items = liteDb.GetCollection<OneDriveItem>();
      return items
         .Query()
         .Where(item => item.PathId == pathId)
         .ToArray();
   }

   public void SaveCachedFiles(IEnumerable<OneDriveItem> children, string pathId)
   {
      using var liteDb = new LiteDatabase(_databaseFile);
      var items = liteDb.GetCollection<OneDriveItem>();
      var staleItems = items
         .Query()
         .Where(oneDriveItem => oneDriveItem.PathId == pathId)
         .ToArray();
      if (staleItems != null && staleItems.Any())
      {
         items.DeleteMany(x => staleItems.Contains(x));
         foreach (var item in staleItems.Where(i => !string.IsNullOrEmpty(i.ThumbnailPath)))
         {
            if (File.Exists(item.ThumbnailPath))
            {
               File.Delete(item.ThumbnailPath);
            }
         }
      }

      foreach (var item in children)
      {
         var findItem = items.FindById(item.Id);
         if (findItem != null)
         {
            items.Update(item);
         }
         else
         {
            items.Insert(item);
         }
      }
   }

   public void UpdateCachedFileById(string itemId, string localFilePath)
   {
      using var liteDb = new LiteDatabase(_databaseFile);
      var items = liteDb.GetCollection<OneDriveItem>();
      var findItem = items.FindById(itemId);
      if (findItem != null)
      {
         findItem.ThumbnailPath = localFilePath;
         items.Update(findItem);
      }
   }
}
